namespace ECS.Components;
public enum Kind { coin, red_mushroom, green_mushroom, flower, star, 
                    minecraft_coin, minecraft_red_mushroom, minecraft_green_mushroom, minecraft_flower, minecraft_star }

public sealed class ItemKind
{
    public Kind Kind;
    public string TextureKey;
    public string SpriteAtlasKey;
}