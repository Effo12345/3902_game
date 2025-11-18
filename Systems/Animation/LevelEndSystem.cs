namespace ECS.Systems;

/// <summary>
/// TODO: Realistically, this class is overcomplicated and overspecific for what it's doing.
/// Try and do it better next time
/// </summary>
public class LevelEndSystem : AbstractSystem
{
    private readonly GraphicsManager Graphics;

    private Entity Player = null;

    public LevelEndSystem(GraphicsManager graphics)
    {
        Graphics = graphics;   
    }

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

        var CurrentLevelEnd = FindLevelEnder();
        if (CurrentLevelEnd is null || CurrentLevelEnd.LevelEnded)
            return;
        
        CurrentLevelEnd.LevelEnded = true;

        DeleteFlagBar();

        SoundPlayer.StopAll();
        SoundPlayer.PlaySfx("CourseClear");

        var player = FindPlayer();

        if (player is not null)
        {
            Player = player;
            // Move player controls to a dummy entity so reset and quit still work
            Engine.Defer(engine => {engine.CreateEntity().AddComponent(player.GetComponent<InputBindings>()); player.RemoveComponent<InputBindings>();});
            player.RemoveComponent<Friction>();
            player.RemoveComponent<Mass>();
            player.GetComponent<Velocity>().Value.X = CurrentLevelEnd.PlayerEndingVelocity.X;
        }
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        var CurrentLevelEnd = FindLevelEnder();

        if (CurrentLevelEnd is null || !CurrentLevelEnd.LevelEnded)
        {
            Graphics.BackgroundColor = CONSTANTS.TEMP.DEFAULT_BACKGROUND_COLOR;
            return;   
        }

        // Get the time when the end of level procedure started
        if (CurrentLevelEnd.LevelEndTime < CONSTANTS.MATH.FLOAT_EPSILON)
        {
            CurrentLevelEnd.LevelEndTime = (float) gameTime.TotalGameTime.TotalSeconds;
            CurrentLevelEnd.BackgroundFadeColor = Graphics.BackgroundColor;
        }

        float deltaSeconds = (float)gameTime.TotalGameTime.TotalSeconds - CurrentLevelEnd.LevelEndTime;

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<SpriteState>(out var sprite) ||
                (entity.TryGetComponent<Tag>(out var tag) && tag.Value == CONSTANTS.TAG.PLAYER)
            )
                continue;

            sprite.Color.R = sprite.Color.G = sprite.Color.B = CurrentLevelEnd.FadeColor;

            if (CurrentLevelEnd.FadeColor > 0)
            {
                float fadePct = (float)(1.0f - (gameTime.TotalGameTime.TotalSeconds - CurrentLevelEnd.LevelEndTime) / CurrentLevelEnd.FadeDuration);

                CurrentLevelEnd.FadeColor = (byte)Math.Clamp(
                    fadePct * CONSTANTS.MATH.BYTE_MAX, 
                    0, 
                    CONSTANTS.MATH.BYTE_MAX
                );

                Graphics.BackgroundColor = new Color(
                    CurrentLevelEnd.BackgroundFadeColor.R / (float)CONSTANTS.MATH.BYTE_MAX * fadePct, 
                    CurrentLevelEnd.BackgroundFadeColor.G / (float)CONSTANTS.MATH.BYTE_MAX * fadePct, 
                    CurrentLevelEnd.BackgroundFadeColor.B / (float)CONSTANTS.MATH.BYTE_MAX * fadePct
                );
            }

            if (deltaSeconds > CurrentLevelEnd.WalkDuration)
            {
                Player.RemoveComponent<Status>();
                Player.RemoveComponent<Acceleration>();
                engine.PubSub.Publish(new AnimationChangeEvent
                {
                    Entity = Player,
                    NewState = CONSTANTS.ANIMATION_STATE.BIG_PEACE_SIGN
                });
                Player.TryGetComponent<Position>(out var pos);
                Player.GetComponent<Position>().Value = new Vector2(pos.Value.X, 300);
                Player.GetComponent<Velocity>().Value = Vector2.Zero;
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

        return null;
    }

    private Entity FindPlayer()
    {
        foreach (var entity in Engine.GetEntities())
        {
            if (entity.TryGetComponent<Tag>(out var tag)&&
                tag.Value == CONSTANTS.TAG.PLAYER
            )
                return entity;
        }

        return null;
    }
}