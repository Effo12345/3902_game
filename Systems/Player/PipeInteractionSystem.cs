namespace ECS.Systems;

public class PipeInteractionSystem : AbstractSystem
{
    private bool Enter = false;
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<PipeEvent>(HandleEntry);
        engine.PubSub.CreateSubscription<InputActionEvent>(HandleInput);
        engine.PubSub.CreateSubscription<InputGamepadStickEvent>(HandleStickInput);
        return true;
    }

    private void HandleInput(IEvent e)
    {
        var inputEvent = (InputActionEvent)e;

        string Action = inputEvent.Action;
        if (Action.Equals(CONSTANTS.ACTION.CROUCH)) 
            Enter = true;
    }

    private void HandleStickInput(IEvent e)
    {
        var stickEvent = (InputGamepadStickEvent)e;

        if (stickEvent.Value.Y > .5)
        {
            Enter = true;
        }
    }

    private void HandleEntry(IEvent e)
    {
        var pipeEvent = (PipeEvent)e;
 
        Direction inputDirection = pipeEvent.InputDirection;
        Direction pipeDirection = pipeEvent.Pipe.EntryDirection;
        Entity entity = pipeEvent.Entity;

        if (entity.TryGetComponent<Position>(out var position) && inputDirection == pipeDirection) 
        {
            Engine.PubSub.Publish(new PipeAnimationEvent
            {
                Entity = entity,
                Pipe = pipeEvent.Pipe
            });
        }
    }
    
    public override void Update(Engine engine, GameTime gameTime)
    {
        Enter = false;
    }
}