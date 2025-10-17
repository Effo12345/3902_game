namespace ECS.Components;

public class CollisionInfo
{
    public List<Collider> Colliders;
}

public struct Collider
{
    public Rectangle collisionBox;
    public enum collisionType
    {
        staticBoxCollider,
        dynamicBoxCollider,
        staticBoxTrigger,
        dynamicBoxTrigger,
        none
    }
}
