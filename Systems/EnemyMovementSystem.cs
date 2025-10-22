using System.Net;

namespace ECS.Systems;

public class EnemyMovementSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        //engine.PubSub.CreateSubscription<InputEvent>(HandleInput);
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
            !entityA.TryGetComponent<IsGrounded>(out var grounded) ||
            !entityA.TryGetComponent<Velocity>(out var velocity) ||
            !entityA.TryGetComponent<FacingDirection>(out var facingDirection)
        )
            return;


        if (direction.Equals(Direction.Top))
        {
            grounded.Value = true;
            velocity.Value.Y = 0;
        }
        if (direction.Equals(Direction.Side))
        {
            velocity.Value.X *= -1;
            facingDirection.IsFacingLeft = !facingDirection.IsFacingLeft;
        }



    }
    public override void Update(Engine engine, GameTime gameTime) { } //No op
}