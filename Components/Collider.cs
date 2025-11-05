namespace ECS.Components;

public enum ColliderType
{
    None,
    Mario,
    Goomba,
    Koopa,
    Shell,
    Block
}

public class Collider
{
    public ColliderType Type;
    public Rectangle HitBox;
}
