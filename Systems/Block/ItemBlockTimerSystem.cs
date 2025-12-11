
namespace ECS.Systems;

public class ItemBlockTimerSystem : AbstractSystem //This is just used for the ten-coin blocks which have a timer
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<HitCount>(out var hitCount))
                continue;

            if (hitCount.Timer > 0)
                hitCount.Timer -= deltaTime;            
        }
    }
}