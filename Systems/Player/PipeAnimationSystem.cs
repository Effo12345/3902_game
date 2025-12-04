
namespace ECS.Systems;

public class PipeAnimationSystem : AbstractSystem
{
    private bool GoingDown = false;
    private bool GoingUp = false;
    private float Timer = 0.0f;
    private Vector2 PipeDestination;
    private Vector2 TravelVelocity;
    private Dictionary<Direction, Vector2> DirectionToVelocity = new Dictionary<Direction, Vector2>
    {
        {Direction.Bottom, new Vector2(0, 100)},
        {Direction.Top, new Vector2(0, -100)},
        {Direction.Left, new Vector2(-100, 0)},
        {Direction.Right, new Vector2(100, 0)},
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
        Vector2 destination = pipeAnimationEvent.Destination;

        if (!entity.TryGetComponent<Status>(out var status))
            return;

        Timer = 0.0f;
        PipeDestination = destination;
        DirectionToVelocity.TryGetValue(status.InputDirection, out var velocity);
        TravelVelocity = velocity;
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

                velocity.Value = TravelVelocity;
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
                velocity.Value = -TravelVelocity;
    
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