
namespace ECS.Systems;

public class CoinCleanupSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Collider>(out var collider) ||
                collider.Type != ColliderType.Coin ||
                !entity.TryGetComponent<Velocity>(out var velocity)) 
                continue;

            if (velocity.Value.Y > 0)
            {
                engine.DestroyEntity(entity);
            }
            

            
        }
    }
}