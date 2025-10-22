namespace ECS.Components;

public class CollisionInfo
{
    public Rectangle HitBox;
    public CollisionType CollisionType;
}

    public enum  CollisionType
    {
        nonMovingBoxCollider,
        movingBoxCollider,
        nonMovingBoxTrigger,
        movingBoxTrigger,
        none
    }
