
using System.Runtime.CompilerServices;

namespace ECS.Systems;

public class CollisionDetectionSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<CollisionBox>(out var collisionBox) ||
                !entity.TryGetComponent<Position>(out var position))
                continue;

        }
    }
}