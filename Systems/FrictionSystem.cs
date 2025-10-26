
namespace ECS.Systems;

public class FrictionSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Friction>(out var friction) ||
                !entity.TryGetComponent<IsGrounded>(out var isGrounded) ||
                !entity.TryGetComponent<Velocity>(out var velocity)
            )
                continue;

            if (!isGrounded.Value)
                continue;

            if (Math.Abs(velocity.Value.X) < friction.Value)
            {
                //velocity.Value.X = 0.0f;
            }
            else
            {
                velocity.Value.X -= friction.Value * Math.Sign(velocity.Value.X);
            }
        }
    }
}