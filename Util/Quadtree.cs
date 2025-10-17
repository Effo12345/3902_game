using System.Net;

namespace ECS.Util;

public class Quadtree
{
    private List<Entity> entities = new();
    private Rectangle Bounds;
    private int X;
    private int Y;
    private int Width;
    private int Height;
    private int sectionCapacity = 2;
    private Quadtree topLeft = null;
    private Quadtree topRight = null;
    private Quadtree bottomLeft = null;
    private Quadtree bottomRight = null;

    private void subdivide()
    {
        Rectangle topLeftBounds = new Rectangle(X, Y, Width / 2, Height / 2);
        Rectangle topRightBounds = new Rectangle(X + Width / 2, Y, Width / 2, Height / 2);
        Rectangle bottomLeftBounds = new Rectangle(X, Y + Height / 2, Width / 2, Height / 2);
        Rectangle bottomRightBounds = new Rectangle(X + Width / 2, Y + Height / 2, Width / 2, Height / 2);

        topLeft = new Quadtree(topLeftBounds);
        topRight = new Quadtree(topRightBounds);
        bottomLeft = new Quadtree(bottomLeftBounds);
        bottomRight = new Quadtree(bottomRightBounds);

        foreach (var entity in entities)
        {
            entities.Remove(entity);
            this.Insert(entity);
        }
    }

    public Quadtree(Rectangle bounds)
    {
        Bounds = bounds;
        X = bounds.X;
        Y = bounds.Y;
        Width = bounds.Width;
        Height = bounds.Height;
    }

    public void Insert(Entity entity)
    {

        if (!entity.TryGetComponent<CollisionInfo>(out var collisionInfo)) //This should never return. Adding something to the quadtree in collisionDetectionSystem assumes the entity has a position.
            return;

        foreach (var collider in collisionInfo.Colliders)
        {
            //If the section already contains the entity, or the collision box is outside the bounds of the quadtree, do nothing.
            if (entities.Contains(entity) || !Bounds.Intersects(collider.collisionBox))
                continue;

            //If this section is under capacity and there arent any subsections, add entity into this section.
            if (entities.Count() < sectionCapacity && topLeft == null) //If topLeft == null, then this section is a leaf node, and can be added to. 
            {
                entities.Add(entity);
                continue;
            }

            //If there aren't any subsections, create subsections and insert into one of the subsections
            if (topLeft == null)
            {
                subdivide();
            }

            topLeft.Insert(entity);
            topRight.Insert(entity);
            bottomLeft.Insert(entity);
            bottomRight.Insert(entity);
        }
    }
    
    public List<CollisionEvent> GetCollisions()
    {
        List<CollisionEvent> collisions = new List<CollisionEvent>();



        return collisions;
    }

}