
namespace ECS.Systems;

public class PhysicsSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<Acceleration>(out var acceleration)
            )
                continue;

            velocity.Value += acceleration.Value * deltaTime;
            position.Value += velocity.Value * deltaTime;
        }
    }
}