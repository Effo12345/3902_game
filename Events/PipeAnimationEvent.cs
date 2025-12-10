
namespace ECS.Events;

public class PipeAnimationEvent : IEvent
{
    public PipeEntry Pipe;
    public Entity Entity;
}