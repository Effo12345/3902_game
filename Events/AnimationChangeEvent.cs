namespace ECS.Events;

public struct AnimationChangeEvent : IEvent
{
    public string NewState;
    public Entity Entity;
}