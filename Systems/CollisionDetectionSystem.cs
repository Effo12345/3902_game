
using System.Runtime.CompilerServices;

namespace ECS.Systems;

public class CollisionDetectionSystem : AbstractSystem
{
    Quadtree quadtree;
    List<CollisionEvent> collisionEvents;
    public override void Update(Engine engine, GameTime gameTime)
    {
        quadtree = new Quadtree(new Rectangle(0,0,800,480)); //TODO: replace the rectangle parameter with game window.
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<CollisionInfo>(out var collisionBox) ||
                !entity.TryGetComponent<Position>(out var position))
                continue;

            quadtree.Insert(entity);
        }

        collisionEvents = quadtree.GetCollisions();
        foreach(var collisionEvent in collisionEvents)
        {
            engine.PubSub.Publish(collisionEvent);
        }

    }
}