public enum ItemKind{ Coin, Mushroom, Flower }

public sealed class ItemBlock
{
    public ItemKind Kind;
    public Texture2D ItemTexture;
    public Rectangle ItemRect;
    public Rectangle UsedBlockRect;
    public bool Used = false;
}