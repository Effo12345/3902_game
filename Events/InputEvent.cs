namespace ECS.Events;

public struct InputEvent : IEvent
{
    public Vector2 ForceVector;
    public Entity Entity;
}