namespace ECS.Events;

public struct CameraLockEvent : IEvent
{
    public bool DoLockCamera;
    public Vector2 LockPos;
}