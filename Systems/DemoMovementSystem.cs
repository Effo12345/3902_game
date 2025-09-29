
namespace ECS.Systems;

public class DemoMovementSystem : AbstractSystem
{
    private float MOVEMENT_MULTIPLIER = 100.0f;

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<InputEvent>(HandleInput);

        return true;
    }

    private void HandleInput(IEvent evt)
    {
        var inputEvent = (InputEvent)evt;
        var entity = inputEvent.Entity;

        if (!entity.TryGetComponent<Velocity>(out var velocity))
            return;

        velocity.Value = inputEvent.MotionVector * MOVEMENT_MULTIPLIER;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        // This line is important! Any time-based stuff needs to be relative to deltatime
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Velocity>(out var velocity)
            )
                continue;

            position.Value += velocity.Value * deltaTime;
        }
    }
}