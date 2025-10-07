namespace ECS.Events;

public struct InputEvent : IEvent
{
    public Vector2 MotionVector;
    public Entity Entity;
}