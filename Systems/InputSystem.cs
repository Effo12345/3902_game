namespace ECS.Systems;

public class InputSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        var keyState = Keyboard.GetState();

        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<InputConfig>(out var inputConfig))
                continue;

            Vector2 outputVector = new();
            foreach (var action in inputConfig.Actions)
            {
                if (action.Keys.Any(key => keyState.IsKeyDown(key)))
                {
                    outputVector += action.Velocity;
                }
            }

            engine.PubSub.Publish(new InputEvent
            {
                MotionVector = outputVector,
                Entity = entity
            });
        }
    }
}