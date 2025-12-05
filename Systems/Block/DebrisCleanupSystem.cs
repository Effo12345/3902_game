namespace ECS.Systems;

public class DebrisCleanupSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Lifetime>(out var life)) 
                continue;

            life.Time -= dt;
            
            if (life.Time <= 0f)
                engine.DestroyEntity(entity);
        }
    }
}