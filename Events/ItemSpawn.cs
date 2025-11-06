namespace ECS.Events;

public class ItemSpawn : IEvent
{
    public Position Position;
    public Entity SourceBlock;
    public ItemKind ItemKind;
    public string TextureKey;
    public string SpriteAtlasKey;
}