namespace ECS.Events;
public struct PipeEvent : IEvent
{
    public Vector2 Destination;
    public Entity Entity;
    public Direction InputDirection;
    public Direction PipeDirection;
}