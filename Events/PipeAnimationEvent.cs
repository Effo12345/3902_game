
namespace ECS.Events;

public class PipeAnimationEvent : IEvent
{
    public Vector2 Destination;
    public Entity Entity;
}