namespace ECS.Systems;

public class NonMarioCollisionSystem : AbstractSystem
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
        movingEntity.TryGetComponent<Collider>(out var movingCollisionInfo);
        secondaryEntity.TryGetComponent<Collider>(out var secondaryCollisionInfo);
        ColliderType movingType = movingCollisionInfo.Type;
        ColliderType secondaryType = secondaryCollisionInfo.Type;

        if (!movingEntity.TryGetComponent<Scale>(out var movingScale) ||
            !movingEntity.TryGetComponent<SpriteState>(out var movingSprite) ||
            !movingEntity.TryGetComponent<Velocity>(out var movingVelocity) ||
            !movingEntity.TryGetComponent<FacingDirection>(out var movingFacingDirection) ||
            !movingEntity.TryGetComponent<Position>(out var movingPosition) ||
            !movingEntity.TryGetComponent<IsGrounded>(out var movingIsGrounded) ||
            !secondaryEntity.TryGetComponent<Position>(out var secondaryPosition) ||
            movingType == ColliderType.Mario
        )
            return;

        //Keep entities on ground
        if (direction.Equals(Direction.Bottom) && secondaryCollisionInfo.Type.Equals(ColliderType.Block))
        {
            movingIsGrounded.Value = true;
            movingPosition.Value.Y = secondaryPosition.Value.Y - movingSprite.SourceRect.Height * movingScale.Value.Y + 1;
        }

        switch (movingType)
        {
            case ColliderType.Goomba:
            case ColliderType.Koopa:
                if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right)) && !secondaryCollisionInfo.Type.Equals(ColliderType.Mario) && !secondaryCollisionInfo.Type.Equals(ColliderType.Shell) && !secondaryEntity.HasComponent<ItemKind>())
                {
                    //Only change direction if moving towards secondaryEntity
                    if ((movingPosition.Value.X < secondaryPosition.Value.X && movingVelocity.Value.X > 0) || (movingPosition.Value.X > secondaryPosition.Value.X) && movingVelocity.Value.X < 0)
                    {
                        movingVelocity.Value.X *= -1;
                        movingFacingDirection.IsFacingLeft = !movingFacingDirection.IsFacingLeft;
                    }
                }
                break;
            case ColliderType.Red_Mushroom:
            case ColliderType.Green_Mushroom:
            case ColliderType.Star:
                if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right)) && secondaryCollisionInfo.Type.Equals(ColliderType.Block))
                {
                    //Only change direction if moving towards secondaryEntity
                    if ((movingPosition.Value.X < secondaryPosition.Value.X && movingVelocity.Value.X > 0) || (movingPosition.Value.X > secondaryPosition.Value.X) && movingVelocity.Value.X < 0)
                    {
                        movingVelocity.Value.X *= -1;
                        movingFacingDirection.IsFacingLeft = !movingFacingDirection.IsFacingLeft;
                    }
                }
                break;
            case ColliderType.Shell:
                if ( //Kills goombas koopas and shells if moving towards them
                    (secondaryType == ColliderType.Goomba || secondaryType == ColliderType.Koopa || secondaryType == ColliderType.Shell) &&
                    ((movingCollisionInfo.HitBox.Center.X < secondaryCollisionInfo.HitBox.Center.X && movingVelocity.Value.X > 0) ||
                    (movingCollisionInfo.HitBox.Center.X > secondaryCollisionInfo.HitBox.Center.X && movingVelocity.Value.X < 0)))
                {
                    secondaryEntity.TryGetComponent<BasicEnemy>(out var secondaryEnemy);
                    secondaryEntity.TryGetComponent<Velocity>(out var secondaryVelocity);
                    secondaryCollisionInfo.HitBox = new Rectangle();
                    secondaryVelocity.Value.X = 0;
                    secondaryVelocity.Value.Y = -200;
                    secondaryEnemy.DeathType = DeathType.Flipped;
                }


                //Change direction only on hitting block
                if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right)) && secondaryCollisionInfo.Type.Equals(ColliderType.Block))
                {
                    //Only change direction if moving towards secondaryEntity
                    if ((movingPosition.Value.X < secondaryPosition.Value.X && movingVelocity.Value.X > 0) || (movingPosition.Value.X > secondaryPosition.Value.X) && movingVelocity.Value.X < 0)
                    {
                        movingVelocity.Value.X *= -1;
                        movingFacingDirection.IsFacingLeft = !movingFacingDirection.IsFacingLeft;
                    }
                }
                break;
        }

        //Make the star jump
        if (movingCollisionInfo.Type == ColliderType.Star && movingIsGrounded.Value == true)
        {
            movingVelocity.Value.Y = -500;
            movingEntity.TryGetComponent<Acceleration>(out var movingAcceleration);
            movingAcceleration.Value.Y = 0;
        }

    }
    
    // no-op
    public override void Update(Engine engine, GameTime gameTime) { }
}