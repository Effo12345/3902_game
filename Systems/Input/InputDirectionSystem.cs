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
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status) || InputThisFrame)
                continue;

            status.InputDirection = Direction.None;

        }
    }
}