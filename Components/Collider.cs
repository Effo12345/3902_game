namespace ECS.Components;

public enum ColliderType
{
    None,
    Mario,
    Goomba,
    Koopa,
    Shell,
    Block,
    Coin,
    Red_Mushroom,
    Green_Mushroom,
    Flower,
    Star,
    Flag
}

public class Collider
{
    public ColliderType Type;
    public Rectangle HitBox;
}
