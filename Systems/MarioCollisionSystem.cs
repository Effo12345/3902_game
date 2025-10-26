
namespace ECS.Systems;

public class MarioCollisionSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<CollisionEvent>(HandleCollision);

        return true;
    }

    public void HandleCollision(IEvent evt)
    {
        var collisionEvent = (CollisionEvent)evt;
        var movingEntity = collisionEvent.MovingEntity;
        var secondaryEntity = collisionEvent.SecondaryEntity;
        Direction direction = collisionEvent.Direction;

        if (!movingEntity.TryGetComponent<CollisionInfo>(out var movingCollisionInfo) ||
            !secondaryEntity.TryGetComponent<CollisionInfo>(out var secondaryCollisionInfo) ||
            !movingEntity.TryGetComponent<IsGrounded>(out var movingIsGrounded) ||
            !movingEntity.TryGetComponent<Position>(out var movingPosition) ||
            !movingEntity.TryGetComponent<Velocity>(out var movingVelocity) ||
            !secondaryEntity.TryGetComponent<Position>(out var secondaryPosition) ||
            !movingEntity.TryGetComponent<InputConfig>(out var config)
            ) //TODO: This is checking if the entity has input config to make sure its mario. It should use collisionType, but that isn't working
            return;


        if (direction.Equals(Direction.Top) && secondaryCollisionInfo.collisionType.Equals(CollisionType.Block))
        {
            movingIsGrounded.Value = true;
            movingPosition.Value.Y = secondaryPosition.Value.Y - movingCollisionInfo.HitBox.Height + 1;
        }
        else if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right)) && secondaryCollisionInfo.collisionType.Equals(CollisionType.Block))
        {
            movingVelocity.Value.X = 0;
            if (movingPosition.Value.X < secondaryPosition.Value.X)
            {
                movingPosition.Value.X = secondaryPosition.Value.X - movingCollisionInfo.HitBox.Width + 1;
            } else
            {
                movingPosition.Value.X = secondaryPosition.Value.X + secondaryCollisionInfo.HitBox.Width;
            }
        }

    }

    public override void Update(Engine engine, GameTime gameTime){} //no op
}