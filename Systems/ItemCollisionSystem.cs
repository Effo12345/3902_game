
namespace ECS.Systems;

public class ItemCollisionSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<CollisionEvent>(HandleCollision);
        return true;
    }
    private void HandleCollision(IEvent evnt)
    {
        var collisionEvent = (CollisionEvent)evnt;
        var movingEntity = collisionEvent.MovingEntity;
        var secondaryEntity = collisionEvent.SecondaryEntity;
        Direction direction = collisionEvent.Direction;

        if (!movingEntity.HasComponent<ItemKind>()) return;

        if (!movingEntity.TryGetComponent<Collider>(out var movingCollisionInfo) ||
            !movingEntity.TryGetComponent<IsGrounded>(out var movingIsGrounded) ||
            !movingEntity.TryGetComponent<Position>(out var movingPosition) ||
            !movingEntity.TryGetComponent<Velocity>(out var movingVelocity) ||
            !secondaryEntity.TryGetComponent<Collider>(out var secondaryCollisionInfo) ||
            !secondaryEntity.TryGetComponent<Position>(out var secondaryPosition)
            )
            return;
        switch(direction)
        {
            case Direction.Top:
                if (movingEntity.TryGetComponent<IsGrounded>(out var grounded))
                    grounded.Value = true;
                movingPosition.Value.Y = secondaryPosition.Value.Y - movingCollisionInfo.HitBox.Height;
                if (movingVelocity.Value.Y > 0f)
                    movingVelocity.Value.Y = 0f;
                break;
            case Direction.Left:
            case Direction.Right:
                if (movingPosition.Value.X < secondaryPosition.Value.X)
                    movingPosition.Value.X = secondaryPosition.Value.X - movingCollisionInfo.HitBox.Width;
                else
                    movingPosition.Value.X = secondaryPosition.Value.X + movingCollisionInfo.HitBox.Width;

                if (movingEntity.TryGetComponent<FacingDirection>(out var face))
                    face.IsFacingLeft = !face.IsFacingLeft;
                movingVelocity.Value.X = -movingVelocity.Value.X;
                break;
        }
    }
    public override void Update(Engine engine, GameTime gameTime){}
}