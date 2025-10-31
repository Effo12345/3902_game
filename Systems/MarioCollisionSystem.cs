using System.Collections;

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

        if (!movingEntity.TryGetComponent<Collider>(out var movingCollisionInfo) ||
            !secondaryEntity.TryGetComponent<Collider>(out var secondaryCollisionInfo) ||
            !movingEntity.TryGetComponent<IsGrounded>(out var movingIsGrounded) ||
            !movingEntity.TryGetComponent<Position>(out var movingPosition) ||
            !movingEntity.TryGetComponent<Velocity>(out var movingVelocity) ||
            !secondaryEntity.TryGetComponent<Position>(out var secondaryPosition) ||
            !movingEntity.TryGetComponent<Status>(out var movingStatus)//TODO: This is checking if the entity has input config to make sure its mario. It should use collisionType, but that isn't working
            ) 
            return;
        ColliderType type = secondaryCollisionInfo.Type;
        switch (type)
        {
            case ColliderType.Block:
                if (direction.Equals(Direction.Bottom))
                {
                    movingIsGrounded.Value = true;
                    movingPosition.Value.Y = secondaryPosition.Value.Y - movingCollisionInfo.HitBox.Height + 1;
                }
                else if (direction.Equals(Direction.Right) || direction.Equals(Direction.Left))
                {
                    movingVelocity.Value.X = 0;
                    if (movingPosition.Value.X < secondaryPosition.Value.X)
                    {
                        movingPosition.Value.X = secondaryPosition.Value.X - movingCollisionInfo.HitBox.Width + 1;
                    }
                    else
                    {
                        movingPosition.Value.X = secondaryPosition.Value.X + secondaryCollisionInfo.HitBox.Width;
                    }
                }
                else if (direction.Equals(Direction.Top))
                {
                    movingVelocity.Value.Y = 0;
                    movingPosition.Value.Y = secondaryPosition.Value.Y + secondaryCollisionInfo.HitBox.Height;
                }
                break;

            case ColliderType.Goomba:
                if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right) || direction.Equals(Direction.Bottom)) && !movingStatus.isInvincible)
                {
                    if (movingStatus.State != "small")
                        movingStatus.State = "small";

                    else
                        Console.WriteLine("DIE DIE DIE"); //TODO: make mario die
                }
                break;
            case ColliderType.Koopa:
                if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right) || direction.Equals(Direction.Bottom)) && !movingStatus.isInvincible)
                {
                    if (movingStatus.State != "small")
                        movingStatus.State = "small";

                    else
                        Console.WriteLine("DIE DIE DIE"); //TODO: make mario die
                }
                break;
        }
    }

    public override void Update(Engine engine, GameTime gameTime){} //no op
}