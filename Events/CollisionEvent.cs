namespace ECS.Events;

public struct CollisionEvent : IEvent
{
    public Rectangle CollisionRectangle;
    public Entity Collider;
    public Entity Collidee;
}