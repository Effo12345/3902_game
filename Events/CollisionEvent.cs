namespace ECS.Events;
public enum Direction {Top, Bottom, Left, Right};
public struct CollisionEvent : IEvent
{
    public Entity MovingEntity;
    public Entity SecondaryEntity;
    public Direction Direction;
}