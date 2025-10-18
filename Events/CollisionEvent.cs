namespace ECS.Events;
public enum Direction {Top, Bottom, Side};
public struct CollisionEvent : IEvent
{
    public Rectangle CollisionRectangle;
    public Entity EntityA;
    public Entity EntityB;
    public bool HurtCollision;
    public Direction Direction;
}