namespace ECS.Systems;

public class ItemChangeSystem : AbstractSystem
{
    private readonly string[] ItemAnimations;
    private int CurrentItemIndex = 0;

    public ItemChangeSystem(string[] items)
    {
        ItemAnimations = items;
    }

    public override bool Initialize(Engine engine)
    {   
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputActionEvent>(HandleInput);

        return true;
    }

    private void HandleInput(IEvent evt)
    {
        var inputEvent = (InputActionEvent)evt;
        var entity = inputEvent.Entity;

        if (inputEvent.Action == "SwitchItem" && inputEvent.Phase == InputPhase.Press)
        {
            CurrentItemIndex = (CurrentItemIndex + 1) % ItemAnimations.Length;

            Engine.PubSub.Publish(new AnimationChangeEvent
            {
                NewState = ItemAnimations[CurrentItemIndex],
                Entity = entity
            });
        }
    }

    public override void Update(Engine engine, GameTime gameTime)
    {    }
}