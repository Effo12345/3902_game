namespace ECS.Events;

public struct InputEvent : IEvent
{
    public Vector2 DirectionVector;
    public Entity Entity;
}