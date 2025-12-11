namespace ECS.Events;
public struct PipeEvent : IEvent
{
    public Entity Entity;
    public Direction InputDirection;
    public PipeEntry Pipe;
}