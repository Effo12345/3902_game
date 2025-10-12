using System.Net;

namespace ECS.Util;

public class Quadtree
{
    private List<Entity> entities = new();
    private Rectangle Bounds;
    private int sectionCapacity = 2;
    private Quadtree topLeft = null;
    private Quadtree topRight = null;
    private Quadtree bottomLeft = null;
    private Quadtree bottomRight = null;

    private void subdivide()
    {
        Rectangle topLeftBounds = new Rectangle(Bounds.X, Bounds.Y, Bounds.Width / 2, Bounds.Height / 2);
        Rectangle topRightBounds = new Rectangle(Bounds.X + Bounds.Width / 2, Bounds.Y, Bounds.Width / 2, Bounds.Height / 2);
        Rectangle bottomLeftBounds = new Rectangle(Bounds.X, Bounds.Y + Bounds.Height / 2, Bounds.Width / 2, Bounds.Height / 2);
        Rectangle bottomRightBounds = new Rectangle(Bounds.X + Bounds.Width / 2, Bounds.Y + Bounds.Height / 2, Bounds.Width / 2, Bounds.Height / 2);

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
    }

    public void Insert(Entity entity)
    {

        if (!entity.TryGetComponent<CollisionBoxList>(out var collisionBoxList)) //This should never return. Adding something to the quadtree in collisionDetectionSystem assumes the entity has a position.
            return;

        foreach (var collisionBox in collisionBoxList.CollisionBoxes)
        {
            if (entities.Contains(entity) || !Bounds.Contains(collisionBox.collisionRectangle.X, collisionBox.collisionRectangle.Y))
            return;

            //If this section is under capacity and there arent any subsections, add entity into this section.
            if (entities.Count() < sectionCapacity && topLeft == null)
            {
                entities.Add(entity);
                return;
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