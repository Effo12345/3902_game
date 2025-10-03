
namespace ECS.Systems;

public class EnemyMovementSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<BasicEnemy>(out var enemy) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<FacingDirection>(out var facingDirection)
            )
                continue;

            position.Value += enemy.Velocity * deltaTime;

            if (position.Value.X <= 0 || position.Value.X >= 752)
            {
                enemy.Velocity.X *= -1;
                facingDirection.IsFacingLeft = !facingDirection.IsFacingLeft;
            }
            
        }

    }
}