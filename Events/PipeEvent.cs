namespace ECS.Events;
public struct PipeEvent : IEvent
{
    public string Destination;
    public Direction CollisionDirection;
    public Direction PipeDirection;
}