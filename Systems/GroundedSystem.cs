
namespace ECS.Systems;

// Temporary grounded system for jumping implementation
// TODO: Will need to replace with collision
public class GroundedSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<CollisionEvent>(HandleCollision);

        return true;
    }

        private void HandleCollision(IEvent evt)
    {
        var collisionEvent = (CollisionEvent)evt;
        var movingEntity = collisionEvent.MovingEntity;
        var secondaryEntity = collisionEvent.SecondaryEntity;
        Direction direction = collisionEvent.Direction;

        if (!movingEntity.TryGetComponent<CollisionInfo>(out var movingCollisionInfo) ||
            !movingEntity.TryGetComponent<IsGrounded>(out var movingIsGrounded) ||
            !movingEntity.TryGetComponent<Position>(out var movingPosition) ||
            !secondaryEntity.TryGetComponent<Position>(out var secondaryPosition))
            return;


        if (direction.Equals(Direction.Top))
        {
            movingIsGrounded.Value = true;
            movingIsGrounded.CoyoteFrames = 4;
            movingPosition.Value.Y = secondaryPosition.Value.Y - movingCollisionInfo.HitBox.Height;
        }
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<IsGrounded>(out var isGrounded) ||
                !entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<Acceleration>(out var acceleration)
            )
                continue;

            if (isGrounded.Value)
            {
                velocity.Value.Y = acceleration.Value.Y = 0.0f;  // This should be done by collision
            }

            //Handle coyote frames
            if (isGrounded.CoyoteFrames > 0)
                isGrounded.CoyoteFrames--;
            else
                isGrounded.Value = false;
        }
    }
}