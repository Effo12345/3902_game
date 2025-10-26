namespace ECS.Components;

public class CollisionInfo
{
    public Rectangle HitBox;
    public CollisionType collisionType;
}

public enum CollisionType
{
    Mario,
    Goomba,
    Koopa,
    Block,
    none
}
