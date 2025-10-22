
using System.Runtime.CompilerServices;

namespace ECS.Systems;

public class CollisionDetectionSystem : AbstractSystem
{
    Quadtree quadtree;
    public override void Update(Engine engine, GameTime gameTime)
    {
        List<CollisionEvent> collisionEvents = new List<CollisionEvent>();
        quadtree = new Quadtree(new Rectangle(0,0,800,480)); //TODO: replace the rectangle parameter with game window.
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<CollisionInfo>(out var collisionInfo))
                continue;

            quadtree.Insert(entity);

            if (!entity.TryGetComponent<Velocity>(out var velocity)) //Only check for collisions on moving entities
                continue;
            collisionEvents.AddRange(quadtree.GetCollisions(entity));
        }

        foreach(var collisionEvent in collisionEvents)
        {
            engine.PubSub.Publish(collisionEvent);
        }

    }
}