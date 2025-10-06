namespace ECS.Events;

public class DebrisSpawn : IEvent
{
    public Position Position;
    public SpriteState Sprite;
    public Break Break;
}
