
namespace ECS.Systems;

public class PipeAnimationSystem : AbstractSystem
{
    private const float PIPE_ENTRY_VEL = 100.0f;

    private bool GoingDown = false;
    private bool GoingUp = false;
    private float Timer = 0.0f;
    private PipeEntry CurrentPipe;
    private Dictionary<Direction, Vector2> DirectionToVelocity = new Dictionary<Direction, Vector2>
    {
        {Direction.Bottom, new Vector2(0, 1)},
        {Direction.Top, new Vector2(0, -1)},
        {Direction.Left, new Vector2(-1, 0)},
        {Direction.Right, new Vector2(1, 0)},
    };

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

        if (!entity.TryGetComponent<Status>(out var status))
            return;

        CurrentPipe = pipeAnimationEvent.Pipe;
        Timer = 0.0f;
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

                DirectionToVelocity.TryGetValue(CurrentPipe.EntryDirection, out var vel);
                velocity.Value = vel * PIPE_ENTRY_VEL;
                if (Timer > 1.5)
                {

                    GoingDown = false;

                    if (CurrentPipe.DoAnimation)
                        GoingUp = true;
                    else
                        RepairPlayer(entity);

                    engine.PubSub.Publish(new CameraLockEvent {DoLockCamera = CurrentPipe.LockCamera, LockPos = CurrentPipe.CameraLockPos});

                    Timer = 0.0f;
                    position.Value.X = CurrentPipe.Destination.X;
                    position.Value.Y = CurrentPipe.Destination.Y + 60;
                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.PIPE_ENTER);
                }
            }
            if (GoingUp)
            {
                Timer += deltaTime;

                DirectionToVelocity.TryGetValue(CurrentPipe.OutputDirection, out var vel);
                velocity.Value = -vel * PIPE_ENTRY_VEL;
    
                if (Timer > 0.5)
                {
                    RepairPlayer(entity);
                    GoingUp = false;
                    Timer = 0;
                }
            }
        }
    }

    private void RepairPlayer(Entity entity)
    {
        entity.AddComponent(new Collider {Type = ColliderType.Mario , IsActive = true});
        entity.AddComponent(new Friction {Value = 6});
        entity.AddComponent(new Mass {Value = 20});
        entity.AddComponent(InputDefinitionLoader.LoadFromFile("Config/InputConfig/player_input.json"));
    }
}