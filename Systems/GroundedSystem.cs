
namespace ECS.Systems;

// Temporary grounded system for jumping implementation
// TODO: Will need to replace with collision
public class GroundedSystem : AbstractSystem
{
    private readonly Vector2 GroundedThreshold;

    public GroundedSystem(Vector2 groundedThreshold)
    {
        GroundedThreshold = groundedThreshold;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<IsGrounded>(out var isGrounded) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<Acceleration>(out var acceleration)
            )
                continue;

            if (position.Value.Y >= GroundedThreshold.Y)
            {
                isGrounded.Value = true;
                velocity.Value.Y = acceleration.Value.Y = 0.0f;  // This should be done by collision
            }
            else
            {
                isGrounded.Value = false;
            }
        }
    }
}