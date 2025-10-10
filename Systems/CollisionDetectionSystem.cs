
using System.Runtime.CompilerServices;

namespace ECS.Systems;

public class CollisionDetectionSystem : AbstractSystem
{
    Quadtree quadtree = new Quadtree(new Rectangle());
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<CollisionBoxList>(out var collisionBox) ||
                !entity.TryGetComponent<Position>(out var position))
                continue;

            
        }
    }
}