namespace ECS.Systems;

public class CollisionDetectionSystem : AbstractSystem
{
    Quadtree quadtree;
    public override void Update(Engine engine, GameTime gameTime)
    {
        List<CollisionEvent> collisionEvents = new List<CollisionEvent>();
        quadtree = new Quadtree(new Rectangle(-200,0,1800,480)); //TODO: replace the rectangle parameter with bounds of level.
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Collider>(out var collisionInfo) ||
                !entity.TryGetComponent<Position>(out var position)
            )
                continue;

            quadtree.Insert(entity);
        }
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Collider>(out var collisionInfo) ||
                !entity.TryGetComponent<Velocity>(out var velocity)) //Only check for collisions on moving entities
                continue;
                
            collisionEvents.AddRange(quadtree.GetCollisions(entity));
        }

        foreach(var collisionEvent in collisionEvents)
        {
            engine.PubSub.Publish(collisionEvent);
        }

    }
}