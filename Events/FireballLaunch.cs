namespace ECS.Events;

public struct FireballLaunch : IEvent
{
    public Position Position;
    public Vector2 Direction;
}