namespace ECS.Components;

public class CollisionBoxList
{
    public List<CollisionBox> CollisionBoxes;
}

public class CollisionBox
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
