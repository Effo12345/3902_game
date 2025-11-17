namespace ECS.Events;

public struct CameraTransformEvent : IEvent
{
    public Matrix Transform;
}