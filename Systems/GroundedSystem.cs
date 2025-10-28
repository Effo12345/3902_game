namespace ECS.Systems;

public class GroundedSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<IsGrounded>(out var isGrounded) ||
                !entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<Acceleration>(out var acceleration)
            )
                continue;

            if (isGrounded.Value && velocity.Value.Y !> 0)
            {
               velocity.Value.Y = acceleration.Value.Y = 0.0f;
            }
            isGrounded.Value = false;
        }
    }
}