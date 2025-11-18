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
                    secondaryVelocity.Value.Y = CONSTANTS.COLLISION.ENEMY_FLIP_VELOCITY;
                    secondaryEnemy.DeathType = DeathType.Flipped;
                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.STOMP);
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
            case ColliderType.Fireball:
                if (secondaryType == ColliderType.Goomba || secondaryType == ColliderType.Koopa || secondaryType == ColliderType.Shell)
                {
                    //Consider making hurt enemy as a seperate system or make the method public
                    Engine.PubSub.Publish(new ScoreAddedEvent{ScoreValue = CONSTANTS.SCORE.ENEMY_VALUE});
                  
                    Engine.DestroyEntity(movingEntity);
                    secondaryEntity.TryGetComponent<BasicEnemy>(out var secondaryEnemy);
                    secondaryEntity.TryGetComponent<Velocity>(out var secondaryVelocity);
                    secondaryCollisionInfo.HitBox = new Rectangle();
                    secondaryVelocity.Value.X = 0;
                    secondaryVelocity.Value.Y = CONSTANTS.COLLISION.ENEMY_FLIP_VELOCITY;
                    secondaryEnemy.DeathType = DeathType.Flipped;
                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.STOMP);

                }
                if ((direction.Equals(Direction.Left) || direction.Equals(Direction.Right)) && secondaryCollisionInfo.Type.Equals(ColliderType.Block))
                {
                    Engine.DestroyEntity(movingEntity);
                    
                }
                break;
        }

        //Make the star and fireball jump
        if ((movingCollisionInfo.Type == ColliderType.Star || movingCollisionInfo.Type == ColliderType.Fireball) && movingIsGrounded.Value == true)
        {
            movingVelocity.Value.Y = CONSTANTS.COLLISION.ITEM_JUMP_VELOCITY;
            movingEntity.TryGetComponent<Acceleration>(out var movingAcceleration);
            movingAcceleration.Value.Y = 0;
        } 

    }

    // no-op
    public override void Update(Engine engine, GameTime gameTime) { }
}