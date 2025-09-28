namespace ECS.Systems;

public class BlockPhysicsSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var e in engine.GetEntities())
        {
            if (e.TryGetComponent<Velocity>(out var v) && e.TryGetComponent<Acceleration>(out var a))
                v.Value += a.Value * dt;

            if (e.TryGetComponent<Position>(out var p) && e.TryGetComponent<Velocity>(out var v2))
                p.Value += v2.Value * dt;
        }
    }
}
