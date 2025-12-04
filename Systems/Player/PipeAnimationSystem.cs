
namespace ECS.Systems;

public class PipeAnimationSystem : AbstractSystem
{
    private bool GoingDown = false;
    private bool GoingUp = false;
    private float Timer = 0.0f;
    private Vector2 PipeDestination;

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<PipeAnimationEvent>(HandleAnimation);
        return true;
    }

    private void HandleAnimation (IEvent e)
    {
        var pipeAnimationEvent = (PipeAnimationEvent)e;

        Entity entity = pipeAnimationEvent.Entity;
        Vector2 destination = pipeAnimationEvent.Destination;

        if (!entity.TryGetComponent<Velocity>(out var velocity) ||
            !entity.TryGetComponent<Acceleration>(out var acceleration) ||
            !entity.TryGetComponent<Collider>(out var collider) ||
            !entity.TryGetComponent<Mass>(out var mass) ||
            !entity.TryGetComponent<Position>(out var position))
            return;

        Timer = 0.0f;
        PipeDestination = destination;
        GoingDown = true;
        SoundPlayer.PlaySfx(CONSTANTS.SOUND.PIPE_ENTER);
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<Acceleration>(out var acceleration))
                continue;

            if (GoingDown)
            {
                Timer += deltaTime;

                //Remove unnecessary components
                entity.RemoveComponent<Collider>();
                entity.RemoveComponent<Friction>();
                entity.RemoveComponent<Mass>();
                entity.RemoveComponent<InputBindings>();

                velocity.Value.Y = 100;
                if (Timer > 1.5)
                {

                    GoingDown = false;
                    GoingUp = true;
                    Timer = 0.0f;
                    position.Value.X = PipeDestination.X;
                    position.Value.Y = PipeDestination.Y + 60;
                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.PIPE_ENTER);
                }
            }
            if (GoingUp)
            {
                Timer += deltaTime;
                velocity.Value.Y = -100;
    
                if (Timer > 0.5)
                {
                    entity.AddComponent(new Collider {Type = ColliderType.Mario , IsActive = true});
                    entity.AddComponent(new Friction {Value = 6});
                    entity.AddComponent(new Mass {Value = 20});
                    entity.AddComponent(InputDefinitionLoader.LoadFromFile("Config/InputConfig/player_input.json"));
                    GoingUp = false;
                    Timer = 0;
                }
            }
        }
    }
}