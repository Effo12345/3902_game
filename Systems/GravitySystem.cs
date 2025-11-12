namespace ECS.Systems;

public class GravitySystem : AbstractSystem
{
    private readonly Vector2 GravityAcceleration = new(0, 100.0f);

    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Mass>(out var mass) ||
                !entity.TryGetComponent<Acceleration>(out var acceleration) ||
                !entity.TryGetComponent<IsGrounded>(out var isGrounded)
            )
                continue;

            // Yes this is fake, but good enough for now
            if (!isGrounded.Value)
                acceleration.Value = mass.Value * GravityAcceleration;
        }
    }
}