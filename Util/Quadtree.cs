using System.Net;

namespace ECS.Util;

public class Quadtree
{
    private readonly List<Entity> entities = new();
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
            this.insert(entity);
            entities.Remove(entity);
        }
    }

    public Quadtree(Rectangle bounds)
    {
        Bounds = bounds;
    }

    public void insert(Entity entity)
    {
        //If this section already contains the entity, don't allow it to be added.
        if (entities.Contains(entity))
            return;

        //If this section is under capacity and there arent any subsections, add entity into this section.
        if (entities.Count() < sectionCapacity && topLeft == null)
        {
            entities.Add(entity);
            return;
        }

        //If there aren't any subsections, create subsections and insert into one of those based on position
        if (topLeft == null)
        {
            subdivide();
        }

        if (!entity.TryGetComponent<Position>(out var position)) //This should never return. Adding something to the quadtree in collisionDetectionSystem assumes the entity has a position.
            return;

        if (position.Value.X <= Bounds.Center.X && position.Value.Y <= Bounds.Center.Y) topLeft.insert(entity); //Center is rounded down, so check for equality on lower havles.
        else if (position.Value.X > Bounds.Center.X && position.Value.Y <= Bounds.Center.Y) topRight.insert(entity);
        else if (position.Value.X <= Bounds.Center.X && position.Value.Y > Bounds.Center.Y) bottomLeft.insert(entity);
        else if (position.Value.X > Bounds.Center.X && position.Value.Y > Bounds.Center.Y) bottomRight.insert(entity);
    }
}