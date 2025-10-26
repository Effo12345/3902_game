
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

            //if (entity.TryGetComponent<InputConfig>(out var input))
            //    Console.Write(velocity.Value.Y);

            velocity.Value += acceleration.Value * deltaTime;
            position.Value += velocity.Value * deltaTime;

            if (entity.TryGetComponent<CollisionInfo>(out var collisionInfo))
            {
                var width = collisionInfo.HitBox.Width;
                var height = collisionInfo.HitBox.Height;

                collisionInfo.HitBox = new Rectangle(position.Value.ToPoint(), new Point(width, height));
            }

            if (!entity.TryGetComponent<VelocityCap>(out var velocityCap))
                continue;

            // TODO: This looks like to could be more efficient
            var vel = velocity.Value;
            var velCap = velocityCap.Value;
            if (Math.Abs(vel.X) > velCap.X)
                vel.X = velCap.X * Math.Sign(vel.X);
            if (Math.Abs(vel.Y) > velCap.Y)
                vel.Y = velCap.Y * Math.Sign(vel.Y);
        }
    }
}