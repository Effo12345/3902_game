namespace ECS.Components;
public enum Kind { coin, red_mushroom, green_mushroom, flower, star }

public sealed class ItemKind
{
    public Kind Kind;
    public string TextureKey;
    public string SpriteAtlasKey;
}