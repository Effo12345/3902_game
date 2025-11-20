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
            !movingEntity.TryGetComponent<Status>(out var movingStatus)
            )
            return;
        ColliderType type = secondaryCollisionInfo.Type;
        secondaryEntity.TryGetComponent<Velocity>(out var secondaryVelocity);
        switch (type)
        {
            case ColliderType.InvisibleBlock:
                if (direction.Equals(Direction.Top) && movingVelocity.Value.Y < 0)
                {
                    movingVelocity.Value.Y = 0;
                    movingPosition.Value.Y = secondaryPosition.Value.Y + secondaryCollisionInfo.HitBox.Height;
                    secondaryCollisionInfo.Type = ColliderType.Block;
                    if (secondaryEntity.TryGetComponent<Visible>(out var visible)) 
                        visible.Value = true;

                    Engine.PubSub.Publish(new BlockBump {Entity = secondaryEntity });
                }
                break;
            case ColliderType.Block:
            case ColliderType.QuestionBlock:
                if (direction.Equals(Direction.Bottom))
                {
                    movingIsGrounded.Value = true;
                    movingPosition.Value.Y = secondaryPosition.Value.Y - movingCollisionInfo.HitBox.Height + 1;
                    if (secondaryEntity.TryGetComponent<PipeEntry>(out var pipeEntry))
                    {
                        Engine.PubSub.Publish(new PipeEvent {Entity = movingEntity, Destination = pipeEntry.Destination, InputDirection = Direction.Bottom, PipeDirection = pipeEntry.EntryDirection });
                    }
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

                    if (secondaryEntity.TryGetComponent<ItemKind>(out var secondaryItem) && secondaryItem.Kind != Kind.coin && secondaryItem.Kind != Kind.green_mushroom && secondaryItem.Kind != Kind.star)
                    {
                        if (movingStatus.State == CONSTANTS.PLAYER_STATE.SMALL)
                        {
                            secondaryItem.Kind = Kind.red_mushroom;
                        } else
                        {
                            secondaryItem.Kind = Kind.flower;
                        }
                    } else if (secondaryEntity.HasComponent<ItemKind>() && secondaryItem.Kind == Kind.coin)
                    {
                        movingStatus.Coins++;
                    }

                    if (secondaryCollisionInfo.Type == ColliderType.QuestionBlock || movingStatus.State == CONSTANTS.PLAYER_STATE.SMALL)
                    {
                        Engine.PubSub.Publish(new BlockBump {Entity = secondaryEntity });   
                    } else
                    {
                        Engine.PubSub.Publish(new BlockBreak {Entity = secondaryEntity });
                    }

                }
                break;
            case ColliderType.Goomba:
                if (movingStatus.StarTimer > 0)
                {
                    HurtEnemy(secondaryEntity,DeathType.Flipped);
                    break;
                }
                if (movingVelocity.Value.Y > 0)
                {
                    movingVelocity.Value.Y = CONSTANTS.COLLISION.ENEMY_BOUNCE_VELOCITY;
                    HurtEnemy(secondaryEntity, DeathType.Squished);
                }
                else if (!movingStatus.isInvincible)
                {
                    HurtMario(movingStatus);
                }
                break;
            case ColliderType.Koopa:
                if (movingStatus.StarTimer > 0)
                {
                    HurtEnemy(secondaryEntity,DeathType.Flipped);
                    break;
                }
                if (movingVelocity.Value.Y > 0)
                {
                    movingVelocity.Value.Y = CONSTANTS.COLLISION.ENEMY_BOUNCE_VELOCITY; //TODO: remove magic number
                    secondaryVelocity.Value.X = 0;
                    secondaryCollisionInfo.Type = ColliderType.Shell;

                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.STOMP);

                    Engine.PubSub.Publish(new AnimationChangeEvent
                    {
                        NewState = CONSTANTS.ANIMATION_STATE.SHELL,
                        Entity = secondaryEntity
                    });
                }
                else if (!movingStatus.isInvincible) //Make sure mario is moving down to kill enemy
                {
                    HurtMario(movingStatus);
                }
                break;
            case ColliderType.Shell:
                if (movingStatus.StarTimer > 0)
                {
                    HurtEnemy(secondaryEntity,DeathType.Flipped);
                    break;
                }
                if (direction.Equals(Direction.Bottom))
                {
                    movingVelocity.Value.Y = CONSTANTS.COLLISION.ENEMY_BOUNCE_VELOCITY; 
                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.SHELL_KICK);
                }

                if (Math.Abs(secondaryVelocity.Value.X) < CONSTANTS.MATH.FLOAT_EPSILON) //If shell is not moving
                {
                    //Compare x position of centers
                    if (movingCollisionInfo.HitBox.Center.X < secondaryCollisionInfo.HitBox.Center.X)
                        secondaryVelocity.Value.X = CONSTANTS.COLLISION.SHELL_KICK_SPEED;
                    else
                        secondaryVelocity.Value.X = -CONSTANTS.COLLISION.SHELL_KICK_SPEED;

                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.SHELL_KICK);
                }
                else
                {
                    if (direction.Equals(Direction.Bottom))
                    {
                        secondaryVelocity.Value.X = 0;
                        SoundPlayer.PlaySfx(CONSTANTS.SOUND.SHELL_KICK);
                    }
                    //Only damage mario if shell is moving towards him and mario isn't moving down
                    else if (!movingStatus.isInvincible && !(movingVelocity.Value.Y > 0) && ((movingCollisionInfo.HitBox.Center.X < secondaryCollisionInfo.HitBox.Center.X && secondaryVelocity.Value.X < 0) || (movingCollisionInfo.HitBox.Center.X > secondaryCollisionInfo.HitBox.Center.X) && secondaryVelocity.Value.X > 0))
                        HurtMario(movingStatus);
                }
                break;
            case ColliderType.Coin:
                Engine.DestroyEntity(secondaryEntity);
                Engine.PubSub.Publish(new CoinPickupEvent{});
                SoundPlayer.PlaySfx(CONSTANTS.SOUND.COIN);
                break;
            case ColliderType.Red_Mushroom:
                if (movingStatus.State != CONSTANTS.PLAYER_STATE.FIRE)
                    movingStatus.State = CONSTANTS.PLAYER_STATE.BIG;
                Engine.DestroyEntity(secondaryEntity);
                SoundPlayer.PlaySfx(CONSTANTS.SOUND.POWERUP);
                break;
            case ColliderType.Flower:
                if (movingStatus.State != CONSTANTS.PLAYER_STATE.SMALL)
                    movingStatus.State = CONSTANTS.PLAYER_STATE.FIRE;   
                else
                    movingStatus.State = CONSTANTS.PLAYER_STATE.BIG;
                Engine.DestroyEntity(secondaryEntity);
                SoundPlayer.PlaySfx(CONSTANTS.SOUND.POWERUP);
                break;
            case ColliderType.Star:
                movingStatus.StarTimer = CONSTANTS.TEMP.STAR_TIMER;
                Engine.DestroyEntity(secondaryEntity);
                SoundPlayer.PlaySfx(CONSTANTS.SOUND.POWERUP);
                break;
            case ColliderType.Green_Mushroom:
                movingStatus.Lives++;
                Engine.DestroyEntity(secondaryEntity);
                SoundPlayer.PlaySfx(CONSTANTS.SOUND.ONEUP);
                break;
        }
    }

    private static void HurtMario(Status status)
    {
        if (status.State != CONSTANTS.PLAYER_STATE.SMALL)
        {
            status.State = CONSTANTS.PLAYER_STATE.SMALL;
            status.isInvincible = true;

            SoundPlayer.PlaySfx(CONSTANTS.SOUND.DAMAGE);
        }

        else
            status.State = CONSTANTS.PLAYER_STATE.DEAD;
    }

    private void HurtEnemy(Entity entity, DeathType deathType)
    {
        entity.TryGetComponent<BasicEnemy>(out var enemy);
        entity.TryGetComponent<Scale>(out var scale);
        entity.TryGetComponent<Velocity>(out var velocity);
        entity.TryGetComponent<Collider>(out var collider);
        collider.HitBox = new Rectangle();
        velocity.Value.X = 0;
        if (deathType == DeathType.Squished) {
            scale.Value = new Vector2(3, 1);
            entity.RemoveComponent<Mass>();
        } else
        {
            velocity.Value.Y = CONSTANTS.COLLISION.ENEMY_FLIP_VELOCITY;
        }
        enemy.DeathType = deathType;
        
        Engine.PubSub.Publish(new ScoreAddedEvent{ScoreValue = CONSTANTS.SCORE.ENEMY_VALUE});

        SoundPlayer.PlaySfx(CONSTANTS.SOUND.STOMP);
    }

    public override void Update(Engine engine, GameTime gameTime){} //no op
}