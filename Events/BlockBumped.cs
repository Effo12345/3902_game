namespace ECS.Events;

public struct BlockBumped : IEvent
{
    public Entity Entity;
    public Position Position;
}