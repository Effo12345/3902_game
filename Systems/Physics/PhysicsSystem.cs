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

            if (!entity.TryGetComponent<VelocityCap>(out var velocityCap))
                continue;

            // TODO: This looks like to could be more efficient
            var velCap = velocityCap.Value;
            if (Math.Abs(velocity.Value.X) > velCap.X)
                velocity.Value.X = velCap.X * Math.Sign(velocity.Value.X);
            if (Math.Abs(velocity.Value.Y) > velCap.Y)
                velocity.Value.Y = velCap.Y * Math.Sign(velocity.Value.Y);
        }
    }
}