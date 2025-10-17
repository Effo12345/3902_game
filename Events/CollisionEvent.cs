namespace ECS.Events;

public struct CollisionEvent : IEvent
{
    public Rectangle CollisionRectangle;
    public enum CollisionType;
    public Entity Collider;
    public Entity Collidee;
}