
namespace ECS.Systems;

public class EnemyCleanupSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<BasicEnemy>(out var enemy) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<AnimationState>(out var animationState))
                continue;

            if (enemy.DeathType == DeathType.Squished)
            {
                enemy.CleanupTimer -= deltaTime;
                animationState.FrameIndex = 0;
            }
            
            if(enemy.CleanupTimer < 0.0f || position.Value.Y > 500)
                engine.DestroyEntity(entity);
            
        }
    }
}