namespace ECS.Systems;

public class PipeInteractionSystem : AbstractSystem
{
    private bool Enter = false;
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<PipeEvent>(HandleEntry);
        engine.PubSub.CreateSubscription<InputActionEvent>(HandleInput);
        return true;
    }

    private void HandleInput(IEvent e)
    {
        var inputEvent = (InputActionEvent)e;

        string Action = inputEvent.Action;
        if (Action.Equals(CONSTANTS.ACTION.CROUCH)) 
            Enter = true;
    }

    private void HandleEntry(IEvent e)
    {
        var pipeEvent = (PipeEvent)e;
 
        Direction inputDirection = pipeEvent.InputDirection;
        Direction pipeDirection = pipeEvent.PipeDirection;
        Entity entity = pipeEvent.Entity;
        Vector2 destination = pipeEvent.Destination;

        if (entity.TryGetComponent<Position>(out var position) && inputDirection == pipeDirection && Enter) 
        {

            Engine.PubSub.Publish(new PipeAnimationEvent
            {
                Entity = entity,
                Destination = destination
            });
        }
    }
    
    public override void Update(Engine engine, GameTime gameTime)
    {
        Enter = false;
    }
}