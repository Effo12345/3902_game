
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
        //If we are grounded and trying to jump, let there be jump
        if (isGrounded.Value && Math.Abs(motionVector.Y - 200.0f) < 0.0001f) //TODO: change this line to more simply detect if the jump button is being pressed, when improved input is added
        {
            isGrounded.Value = false;
        }
         if (!isGrounded.Value) //If we are not grounded and trying to jump, let there not be jump
             motionVector.Y = 0.0f;

        velocity.Value += motionVector;
        //isGrounded.Value = false;
    }

    // no-op
    public override void Update(Engine engine, GameTime gameTime) {}
}