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
                if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right) || direction.Equals(Direction.Top)) && !movingStatus.isInvincible)
                {
                    HandleDamage(movingStatus);
                }
                else if (direction.Equals(Direction.Bottom) && movingVelocity.Value.Y > 0)
                {
                    movingVelocity.Value.Y = -500; //TODO: remove magic number
                    secondaryEntity.TryGetComponent<BasicEnemy>(out var secondaryEnemy);
                    secondaryEnemy.isDead = true;
                    secondaryCollisionInfo.HitBox = new Rectangle();
                }
                break;
            case ColliderType.Koopa:
                if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right) || direction.Equals(Direction.Top)) && !movingStatus.isInvincible)
                {
                    HandleDamage(movingStatus);
                }
                else if (direction.Equals(Direction.Bottom) && movingVelocity.Value.Y > 0) //Make sure mario is moving down to kill enemy
                {
                    movingVelocity.Value.Y = -500; //TODO: remove magic number
                    secondaryEntity.TryGetComponent<Velocity>(out var secondaryVelocity);
                    secondaryVelocity.Value.X = 0;
                    secondaryCollisionInfo.Type = ColliderType.Shell;
                    Engine.PubSub.Publish(new AnimationChangeEvent
                    {
                        NewState = "shell",
                        Entity = secondaryEntity
                    });
                }
                break;
        }
    }

    private void HandleDamage(Status status)
    {
        if (status.State != "small")
        {
            status.State = "small";
            status.isInvincible = true;
        }

        else
            status.State = "dead";
    }

    public override void Update(Engine engine, GameTime gameTime){} //no op
}