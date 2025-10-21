public enum ItemKind{ SuperMushroom, FireFlower, SuperStar}

public sealed class ItemBlock
{
    public ItemKind Kind;
    public Rectangle ItemRect;
    public Texture2D ItemTexture;
    public Rectangle UsedBlockRect;
    public bool Used = false;
}