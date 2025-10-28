namespace ECS.Components;

public enum ColliderType
{
    None,
    Mario,
    Goomba,
    Koopa,
    Block
}

public class Collider
{
    public ColliderType Type;
    public Rectangle HitBox;
}
