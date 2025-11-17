
namespace ECS.Systems;

public class LevelEndSystem : AbstractSystem
{
    private LevelEndParams CurrentLevelEnd = null;

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);

        engine.PubSub.CreateSubscription<CollisionEvent>(HandleCollision);

        return true;
    }

    public void HandleCollision(IEvent evt)
    {
        var collisionEvent = (CollisionEvent)evt;
        var movingEntity = collisionEvent.MovingEntity;
        var secondaryEntity = collisionEvent.SecondaryEntity;

        if (!movingEntity.TryGetComponent<Tag>(out var movingTag) ||
            movingTag.Value != CONSTANTS.TAG.PLAYER ||
            !secondaryEntity.TryGetComponent<Tag>(out var secondaryTag) ||
            secondaryTag.Value != CONSTANTS.TAG.FLAG
        )
            return;

        DeleteFlagBar();
        CurrentLevelEnd = FindLevelEnder();

        if (CurrentLevelEnd is not null)
            CurrentLevelEnd.LevelEnded = true;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        if (CurrentLevelEnd is null || !CurrentLevelEnd.LevelEnded)
            return;

        // Get the time when the end of level procedure started
        if (CurrentLevelEnd.LevelEndTime < CONSTANTS.MATH.FLOAT_EPSILON)
            CurrentLevelEnd.LevelEndTime = (float) gameTime.TotalGameTime.TotalSeconds;

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<SpriteState>(out var sprite) ||
                (entity.TryGetComponent<Tag>(out var tag) && tag.Value == CONSTANTS.TAG.PLAYER)
            )
                continue;

            sprite.Color.R = sprite.Color.G = sprite.Color.B = CurrentLevelEnd.FadeColor;

            if (CurrentLevelEnd.FadeColor > 0)
            {
                CurrentLevelEnd.FadeColor = (byte)Math.Clamp(
                    CONSTANTS.MATH.BYTE_MAX - (CONSTANTS.MATH.BYTE_MAX * ((gameTime.TotalGameTime.TotalSeconds - CurrentLevelEnd.LevelEndTime) / CurrentLevelEnd.FadeDuration)), 
                    0, 
                    CONSTANTS.MATH.BYTE_MAX
                );
            }
        }
    }

    private void DeleteFlagBar()
    {
        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<Tag>(out var tag) ||
                tag.Value != CONSTANTS.TAG.FLAG_BAR
            )
                continue;
            
            Engine.DestroyEntity(entity);
        }
    }

    private LevelEndParams FindLevelEnder()
    {
        foreach (var entity in Engine.GetEntities())
        {
            if (entity.TryGetComponent<LevelEndParams>(out var data))
                return data;
        }

        // TODO: Handle this case properly
        return null;
    }
}