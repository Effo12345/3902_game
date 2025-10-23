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


        if (direction.Equals(Direction.Bottom))
        {
            isGroundedA.Value = true;
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
            if (collisionInfoB.CollisionType.Equals(CollisionType.movingBoxCollider))
            {
                entityB.TryGetComponent<Velocity>(out var velocityB);
                entityB.TryGetComponent<FacingDirection>(out var facingDirectionB);
                velocityB.Value.X *= -1;
                facingDirectionB.IsFacingLeft = !facingDirectionB.IsFacingLeft;
            }
        }



    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (entity.TryGetComponent<BasicEnemy>(out var enemy))
            {
                entity.TryGetComponent<CollisionInfo>(out var collisionInfo);
                //Console.Write(collisionInfo.HitBox.ToString());
            }
        }
    } //No op
}