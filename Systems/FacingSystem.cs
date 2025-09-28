
namespace ECS.Systems;

public class FacingSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputEvent>(HandleInput);

        return true;
    }

    private void HandleInput(IEvent evt)
    {
        var inputEvent = (InputEvent)evt;

        if (!inputEvent.Entity.TryGetComponent<FacingDirection>(out var facingDirection))
            return;

        float horizontalMotion = inputEvent.MotionVector.X;
        // Don't change direction without input
        if (Math.Abs(horizontalMotion) <= 0.01f)
            return;

        facingDirection.IsFacingLeft = horizontalMotion < 0;
    }

    // no-op
    public override void Update(Engine engine, GameTime gameTime) { }
}