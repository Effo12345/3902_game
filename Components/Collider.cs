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
    Coin,
    Red_Mushroom,
    Green_Mushroom,
    Flower,
    Star,
    Flag
    Pipe,
    Fireball,
}

public class Collider
{
    public ColliderType Type;
    public Rectangle HitBox;
}
