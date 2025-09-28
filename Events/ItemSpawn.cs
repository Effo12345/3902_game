namespace ECS.Events;

public struct ItemSpawn : IEvent
{
    public Position Position;
    public Texture2D Texture;
    public ItemKind Kind;
    public Rectangle SourceRect;
}