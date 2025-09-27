
namespace ECS.Systems;

public class GravitySystem : AbstractSystem
{
    private readonly Vector2 GravityAcceleration = new(0, 10.0f);

    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Mass>(out var mass) ||
                !entity.TryGetComponent<Acceleration>(out var acceleration)
            )
                continue;

            // Yes this is fake, but good enough for now
            // acceleration.Value += mass.Value * GravityAcceleration * deltaTime;
            acceleration.Value += GravityAcceleration;
        }
    }
}