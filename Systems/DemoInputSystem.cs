
using System.Linq;

namespace ECS.Systems;

public class DemoInputSystem : AbstractSystem
{
    private Dictionary<Keys, Vector2> DemoMotionMap = new Dictionary<Keys, Vector2>
    {
        {Keys.Left, new Vector2(-1, 0)},
        {Keys.Right, new Vector2(1, 0)},
        {Keys.Up, new Vector2(0, -1)},
        {Keys.Down, new Vector2(0, 1)}
    };

    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<Velocity>(out var velocity))
                continue;

            var direction = Vector2.Zero;

            List<Vector2> directionVectors = Keyboard.GetState().GetPressedKeys()
                .Where(key => DemoMotionMap.ContainsKey(key))
                .Select(key => DemoMotionMap[key])
                .ToList();

            foreach (var vector in directionVectors)
            {
                direction += vector;
            }

            Engine.PubSub.Publish(new InputEvent
            {
                MotionVector = direction,
                Entity = entity
            });
        }
    }
}