namespace ECS.Systems;

public class ItemChangeSystem : AbstractSystem
{
    private readonly string[] ItemAnimations;
    private readonly float MagicKeyVector = 1.0f;
    private readonly float Epsilon = 0.0001f;
    private readonly int InputBufferLength = 5;  // frames

    private int CurrentItemIndex = 0;
    // Buffer inputs coming in to work around not having 
    // pressed vs. pressing from input system
    private int InputBuffer;

    public ItemChangeSystem(string[] items)
    {
        ItemAnimations = items;
    }

    public override bool Initialize(Engine engine)
    {   
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputEvent>(HandleInput);

        return true;
    }

    private void HandleInput(IEvent evt)
    {
        var inputEvent = (InputEvent)evt;
        var motionVector = inputEvent.MotionVector;
        var entity = inputEvent.Entity;

        // Reject unimportant inputs
        if (Math.Abs(motionVector.Y - MagicKeyVector) > Epsilon ||
            entity.HasComponent<BasicEnemy>()
        )
            return;

        if (InputBuffer > 0)
        {
            InputBuffer = InputBufferLength;
            return;
        }

        CurrentItemIndex = (CurrentItemIndex + 1) % ItemAnimations.Length;

        Engine.PubSub.Publish(new AnimationChangeEvent
        {
            NewState = ItemAnimations[CurrentItemIndex],
            Entity = entity
        });

        InputBuffer = InputBufferLength;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        if (InputBuffer > 0)
            InputBuffer--;
    }
}