

using System.Runtime.CompilerServices;
using Microsoft.VisualBasic;

namespace ECS.Util;

//This class is used to quickly and easily create an enemy with some set properties: Default speed -50 left, starts facing left, scaled 3x3, no acceleration, no rotation
//and animationConfig conventional for a goomba or a koopa.
public static class EnemyFactory
{
    private static Vector2 enemyVelocity = new Vector2(-100, 0);

    private static Entity entity;

    //This method assumes some naming conventions in the animation config file, namely that it will have a state called "walking".
    public static Entity makeEnemy(Engine engine, Texture2D texture, AnimationConfig animationConfig, String name, Vector2 position)
    {
        entity = engine.CreateEntity();

        entity.AddComponent(new Position { Value = position });
        entity.AddComponent(new BasicEnemy { Name = name, Velocity = enemyVelocity });
        entity.AddComponent(new FacingDirection { IsFacingLeft = true });
        entity.AddComponent(new Rotation { Value = 0.0f });
        entity.AddComponent(new Scale { Value = new Vector2(3, 3) });
        entity.AddComponent(new SpriteState { Texture = texture, Color = Color.White });
        entity.AddComponent(new AnimationConfig { States = animationConfig.States });
        entity.AddComponent(new AnimationState { CurrentState = "walking" });

        return entity;
    }

    //In these two methods the position is removed instead of toggling a visibility component, because even if the visibility component is false the enitity is still updated.
    public static void hideEnemy(Entity entity)
    {
        if (entity.TryGetComponent<Position>(out var position))
        {
            entity.RemoveComponent<Position>();
        }
    }
    public static void resetEnemy(Entity entity, Vector2 position)
    {
        if (!entity.TryGetComponent<Position>(out var value) && entity.TryGetComponent<FacingDirection>(out var facingDirection) && entity.TryGetComponent<BasicEnemy>(out var enemy))
        {
            entity.AddComponent(new Position { Value = position });
            facingDirection.IsFacingLeft = true;
            enemy.Velocity = enemyVelocity;
        }
    }
}