namespace ECS.Systems;

public class BlockPhysicsSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //loop through entities
        foreach (var e in engine.GetEntities())
        {
            //update its velocity
            if (e.TryGetComponent<Velocity>(out var v) && e.TryGetComponent<Acceleration>(out var a))
                v.Value += a.Value * dt;
            //update its position
            if (e.TryGetComponent<Position>(out var p) && e.TryGetComponent<Velocity>(out var v2))
                p.Value += v2.Value * dt;
        }
    }
}
