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

        Rectangle[] collisionBoxes = {collisionInfo.HitBox, collisionInfo.HurtBox};

        foreach (var collisionBox in collisionBoxes)
        {
            //If the section already contains the entity, or the collision box is outside the bounds of the quadtree, do nothing.
            if (entities.Contains(entity) || !Bounds.Intersects(collisionBox))
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

        //If this section has subsections, parse the subsections instead: only leaf sections have data.
        if (topLeft != null)
        {
            collisions.AddRange(topLeft.GetCollisions());
            collisions.AddRange(topRight.GetCollisions());
            collisions.AddRange(bottomLeft.GetCollisions());
            collisions.AddRange(bottomRight.GetCollisions());
        }
        else { //If this section is a leaf section, check for collisions and add collisionEvents to the list.
            Entity entityA;
            Entity entityB;
            Rectangle intersection;
            CollisionEvent collisionEvent;
            for (int i = 0; i < entities.Count; i++)
            {
                if (!entities[i].HasComponent<Velocity>()) //If the entity isn't movable, skip to check next entity.
                    continue;
                    
                entityA = entities[i];
                for(int j = i+1; j < entities.Count; j++)
                {
                    entityB = entities[j];

                    //Get collision info for both entities
                    entityA.TryGetComponent<CollisionInfo>(out var collisionInfoA);
                    entityB.TryGetComponent<CollisionInfo>(out var collisionInfoB);

                    //Look for collisionbox collision
                    intersection = Rectangle.Intersect(collisionInfoA.HitBox, collisionInfoB.HitBox);
                    Direction direction;
                    if (intersection.Width != 0 || intersection.Height != 0)
                    {
                        if (intersection.Width >= intersection.Height)
                        {
                            if (collisionInfoA.HitBox.Y > collisionInfoB.HitBox.Y)
                            {
                                direction = Direction.Top;
                            }
                            else
                            {
                                direction = Direction.Bottom;
                            }
                        }
                        else
                        {
                            direction = Direction.Side;
                        }
                        collisionEvent = new CollisionEvent
                        {
                            EntityA = entityA,
                            EntityB = entityB,
                            CollisionRectangle = intersection,
                            HurtCollision = false,
                            Direction = direction
                        };
                        collisions.Add(collisionEvent);
                    }

                    //Look for hurtbox collision
                    intersection = Rectangle.Intersect(collisionInfoA.HurtBox, collisionInfoB.HurtBox);
                    if (intersection.Width != 0 || intersection.Height != 0)
                    {
                        if (intersection.Width >= intersection.Height)
                        {
                            if (collisionInfoA.HitBox.Y > collisionInfoB.HitBox.Y)
                            {
                                direction = Direction.Top;
                            }
                            else
                            {
                                direction = Direction.Bottom;
                            }
                        }
                        else
                        {
                            direction = Direction.Side;
                        }
                        collisionEvent = new CollisionEvent
                        {
                            EntityA = entityA,
                            EntityB = entityB,
                            CollisionRectangle = intersection,
                            HurtCollision = true,
                            Direction = direction
                        };
                        collisions.Add(collisionEvent);
                    }
                }
            }
            
        }


        return collisions;
    }

}