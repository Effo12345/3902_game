namespace ECS.Components;

public class CollisionInfo
{
    public Rectangle HitBox;
    public Rectangle HurtBox;

    public enum CollisionType
    {
        nonMovingBoxCollider,
        movingBoxCollider,
        nonMovingBoxTrigger,
        movingBoxTrigger,
        none
    }}

