namespace ECS.Components;

public enum ColliderType
{
    None,
    Mario,
    Goomba,
    Koopa,
    Shell,
    Block,
    QuestionBlock,
    HardBlock,
    InvisibleBlock,
    Coin,
    Red_Mushroom,
    Green_Mushroom,
    Flower,
    Star,
    Flag,
    Pipe,
    Fireball,
    Water
}

public class Collider
{
    public ColliderType Type;
    public Rectangle HitBox;

    public bool IsActive;
}
