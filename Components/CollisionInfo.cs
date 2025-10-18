namespace ECS.Components;

public class CollisionInfo
{
    public Collider HitBox;
    public Collider HurtBox;
}

public struct Collider
{
    public Rectangle CollisionBox;
    public enum CollisionType
    {
        staticBoxCollider,
        dynamicBoxCollider,
        staticBoxTrigger,
        dynamicBoxTrigger,
        none
    }
}
