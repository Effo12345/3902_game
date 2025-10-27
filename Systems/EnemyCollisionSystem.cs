namespace ECS.Systems;

public class EnemyCollisionSystem : AbstractSystem
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
        movingEntity.TryGetComponent<CollisionInfo>(out var movingCollisionInfo);
        secondaryEntity.TryGetComponent<CollisionInfo>(out var secondaryCollisionInfo);

        if (!movingEntity.TryGetComponent<BasicEnemy>(out var movingEnemy) ||
            !movingEntity.TryGetComponent<Velocity>(out var movingVelocity) ||
            !movingEntity.TryGetComponent<FacingDirection>(out var movingFacingDirection) ||
            !movingEntity.TryGetComponent<Position>(out var movingPosition) ||
            !movingEntity.TryGetComponent<IsGrounded>(out var movingIsGrounded) ||
            !secondaryEntity.TryGetComponent<Position>(out var secondaryPosition)
        )
            return;

        if (direction.Equals(Direction.Top) && secondaryCollisionInfo.collisionType.Equals(CollisionType.Block))
        {
            movingIsGrounded.Value = true;
            movingPosition.Value.Y = secondaryPosition.Value.Y - movingCollisionInfo.HitBox.Height + 1;
        }
        else if (direction.Equals(Direction.Left) || direction.Equals(Direction.Right))
        {
            //Only change direction if moving towards secondaryEntity
            if ((movingPosition.Value.X < secondaryPosition.Value.X && movingVelocity.Value.X > 0) || (movingPosition.Value.X > secondaryPosition.Value.X) && movingVelocity.Value.X < 0)
            {
                movingVelocity.Value.X *= -1;
                movingFacingDirection.IsFacingLeft = !movingFacingDirection.IsFacingLeft;
            }
        }
        else if (direction.Equals(Direction.Bottom) && secondaryCollisionInfo.collisionType.Equals(CollisionType.Mario))
        {
            movingEnemy.isDead = true;
            movingCollisionInfo.HitBox = new Rectangle();
        }
    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        // float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // foreach (var entity in Engine.GetEntities())
        // {
        //     if (!entity.TryGetComponent<BasicEnemy>(out var enemy) ||
        //         !entity.TryGetComponent<Position>(out var position) ||
        //         !entity.TryGetComponent<Velocity>(out var velocity) ||
        //         !entity.TryGetComponent<FacingDirection>(out var facingDirection)
        //     )
        //         continue;


        //     if (position.Value.X <= 0 || position.Value.X >= 550) //TODO: remove magic numbers
        //     {
        //         velocity.Value.X *= -1;
        //         facingDirection.IsFacingLeft = !facingDirection.IsFacingLeft;
        //     }
        // }
    }
}