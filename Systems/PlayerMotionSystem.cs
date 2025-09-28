
namespace ECS.Systems;

public class PlayerMotionSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<InputEvent>(HandleInput);

        return true;
    }

    private void HandleInput(IEvent evt)
    {
        var inputEvent = (InputEvent)evt;
        var motionVector = inputEvent.MotionVector;
        var entity = inputEvent.Entity;

        if (!entity.TryGetComponent<Velocity>(out var velocity) ||
            !entity.TryGetComponent<IsGrounded>(out var isGrounded)
        )
            return;

        // TODO: Relate button pressed length to jump height
        if (!isGrounded.Value)
            motionVector.Y = 0.0f;

        velocity.Value += motionVector;
    }

    // no-op
    public override void Update(Engine engine, GameTime gameTime) { }
}