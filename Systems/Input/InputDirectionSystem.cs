namespace ECS.Systems;

public class InputDirectionSystem : AbstractSystem
{
    private bool InputThisFrame = false;
    private Dictionary<String,Direction> InputToDirection = new Dictionary<string, Direction>
    {
        {"MoveLeft", Direction.Left},
        {"MoveRight", Direction.Right},
        {"Crouch" , Direction.Bottom}
    };

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputActionEvent>(HandleInput);
        engine.PubSub.CreateSubscription<InputAxisEvent>(HandleInputAxis);

        return true;
    }

    private void HandleInput(IEvent e)
    {
        var InputEvent = (InputActionEvent)e;

        string Action = InputEvent.Action;
        Entity entity = InputEvent.Entity;
        InputThisFrame = true;

        if (entity.TryGetComponent<Status>(out var status))
        {
            InputToDirection.TryGetValue(Action, out var InputDirection);
            status.InputDirection = InputDirection;
        }
    }

    private void HandleInputAxis(IEvent e)
    {
        var axisEvent = (InputAxisEvent)e;
        Entity entity = axisEvent.Entity;

        if (entity.TryGetComponent<Status>(out var status))
        {
            if (Math.Abs(axisEvent.Value.X) > CONSTANTS.MATH.FLOAT_EPSILON)
                status.InputDirection = axisEvent.Value.X > 0 ? Direction.Right : Direction.Left;
        }
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status))
                continue;
            
            if (InputThisFrame)
                InputThisFrame = false;

        }
    }
}