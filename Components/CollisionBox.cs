namespace ECS;

public class CollisionBox
{
    public Rectangle collisionRectangle;
    public enum collisionType
    {
        staticBoxCollider,
        dynamicBoxCollider,
        staticBoxTrigger,
        dynamicBoxTrigger
    }
}