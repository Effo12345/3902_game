namespace ECS.Components;

public class CollisionBoxList
{
    public List<CollisionBox> CollisionBoxes;
}

public struct CollisionBox
{
    public Rectangle collisionRectangle;
    public enum collisionType
    {
        staticBoxCollider,
        dynamicBoxCollider,
        staticBoxTrigger,
        dynamicBoxTrigger,
        none
    }
}
