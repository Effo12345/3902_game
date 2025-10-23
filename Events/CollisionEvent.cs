namespace ECS.Events;
public enum Direction {Top, Bottom, Side};
public struct CollisionEvent : IEvent
{
    public Entity EntityA;
    public Entity EntityB;
    public Direction Direction;
}