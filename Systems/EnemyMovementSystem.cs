using System.Net;

namespace ECS.Systems;

public class EnemyMovementSystem : AbstractSystem
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
        var entityA = collisionEvent.EntityA;
        var entityB = collisionEvent.EntityB;
        Direction direction = collisionEvent.Direction;
        entityA.TryGetComponent<CollisionInfo>(out var collisionInfoA);
        entityB.TryGetComponent<CollisionInfo>(out var collisionInfoB);

        if (!entityA.TryGetComponent<BasicEnemy>(out var enemy) ||
            !entityA.TryGetComponent<IsGrounded>(out var isGroundedA) ||
            !entityA.TryGetComponent<Velocity>(out var velocityA) ||
            !entityA.TryGetComponent<FacingDirection>(out var facingDirectionA)
        )
            return;


        if (direction.Equals(Direction.Top))
        {
            isGroundedA.Value = true;
            entityA.TryGetComponent<Position>(out var positionA);
            entityB.TryGetComponent<Position>(out var positionB);

            positionA.Value.Y = positionB.Value.Y - collisionInfoA.HitBox.Height;
        }
        if (direction.Equals(Direction.Side))
        {
            //Only change direction if moving towards entityB
            entityA.TryGetComponent<Position>(out var positionA);
            entityB.TryGetComponent<Position>(out var positionB);
            if ((positionA.Value.X < positionB.Value.X && velocityA.Value.X > 0) || (positionA.Value.X > positionB.Value.X) && velocityA.Value.X < 0)
            {
                velocityA.Value.X *= -1;
                facingDirectionA.IsFacingLeft = !facingDirectionA.IsFacingLeft;
            }
        }
    }
    public override void Update(Engine engine, GameTime gameTime){}//No op
}