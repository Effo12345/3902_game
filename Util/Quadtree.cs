namespace ECS.Util;

public class Quadtree
{
    private List<Entity> entities = new();
    private Rectangle Bounds;
    private int X;
    private int Y;
    private int Width;
    private int Height;
    private int sectionCapacity = 4;
    private Quadtree topLeft = null;
    private Quadtree topRight = null;
    private Quadtree bottomLeft = null;
    private Quadtree bottomRight = null;

    private bool intersectsQuadrants(Rectangle collisionBox)
    {
        bool overlaps = false;
        if (collisionBox.X <= X + Width / 2 && collisionBox.X + collisionBox.Width >= X + Width / 2 ||  //If rectangle intersects multiple quadtrants, keep in this node instead of pushing to quadrants.
            collisionBox.Y <= Y + Height / 2 && collisionBox.Y + collisionBox.Height >= Y + Height / 2)
            {
                overlaps = true;
            }
        return overlaps;
    }
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

        List<Entity> dummyEntities = [.. entities];
        foreach (var entity in dummyEntities)
        {
            entity.TryGetComponent<Collider>(out var collisionInfo);
            Rectangle collisionBox = collisionInfo.HitBox;

            if (!intersectsQuadrants(collisionBox))
            {
                topLeft.Insert(entity);
                topRight.Insert(entity);
                bottomLeft.Insert(entity);
                bottomRight.Insert(entity);
                entities.Remove(entity);
            }
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
        if (!entity.TryGetComponent<Collider>(out var collisionInfo)) //This should never return. Adding something to the quadtree in collisionDetectionSystem assumes the entity has a position.
            return;
            
        Rectangle collisionBox = collisionInfo.HitBox;
        //If the section already contains the entity, or the collision box is outside the bounds of the quadtree, do nothing.
        if (entities.Contains(entity) || !Bounds.Intersects(collisionBox))
            return;

            //If this section is under capacity and either there arent any subsections or the collision box overlaps with those subsections, add entity into this section.

        if (entities.Count() < sectionCapacity && (topLeft == null || intersectsQuadrants(collisionBox))) //If topLeft == null, then this section is a leaf node, and can be added to. 
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
    
    
    public List<CollisionEvent> GetCollisions(Entity movingEntity)
    {
        List<CollisionEvent> collisions = new List<CollisionEvent>();
        List<Quadtree> children = new List<Quadtree> { topLeft, topRight, bottomLeft, bottomRight };
        movingEntity.TryGetComponent<Collider>(out var movingCollisionInfo);
        Rectangle movingCollisionBox = movingCollisionInfo.HitBox;

        //First, get collisions in child trees
        if (topLeft != null)
        {
            foreach (var childQuadtree in children)
            {
                if (movingCollisionBox.Intersects(childQuadtree.Bounds))
                {
                    collisions.AddRange(childQuadtree.GetCollisions(movingEntity));
                }
            }
        }
        
        //Now get collisions for rectangles in this quadtree
        foreach (var secondaryEntity in entities)
        {
            secondaryEntity.TryGetComponent<Collider>(out var secondaryCollisionInfo);
            Rectangle secondaryCollisionBox = secondaryCollisionInfo.HitBox;

            if (movingCollisionBox.Intersects(secondaryCollisionBox) && !movingEntity.Equals(secondaryEntity))
            {
                Rectangle intersection = Rectangle.Intersect(movingCollisionBox, secondaryCollisionBox);
                Direction direction;
                if (intersection.Width >= intersection.Height)
                {
                    if (movingCollisionBox.Y < secondaryCollisionBox.Y)
                    {
                        direction = Direction.Bottom;
                    }
                    else
                    {
                        direction = Direction.Top;
                    }
                }
                else
                {
                    if (movingCollisionBox.X < secondaryCollisionBox.X)
                    {
                        direction = Direction.Right;
                    }
                    else
                    {
                        direction = Direction.Left;
                    }
                }
                CollisionEvent collisionEvent = new CollisionEvent
                {
                    MovingEntity = movingEntity,
                    SecondaryEntity = secondaryEntity,
                    Direction = direction
                };
                collisions.Add(collisionEvent);
            }
            
        }

        return collisions;
    }
}