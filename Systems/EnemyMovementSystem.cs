using System.Net;

namespace ECS.Systems;

public class EnemyMovementSystem : AbstractSystem
{
    private Vector2 TestEnemyPosition;
    private Vector2 TestEnemyVelocity;

    private int EnemyIndex = 0;

    public EnemyMovementSystem(Vector2 testEnemyPosition, Vector2 testEnemyVelocity)
    {
        TestEnemyPosition = testEnemyPosition;
        TestEnemyVelocity = testEnemyVelocity;
    }
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputActionEvent>(HandleInput);
        engine.PubSub.CreateSubscription<CollisionEvent>(HandleCollision);

        return true;
    }
    private void HandleInput(IEvent evt)
    {
        var inputEvent = (InputActionEvent)evt;
        var entity = inputEvent.Entity;

        if (inputEvent.Action == "SwitchEnemy" && inputEvent.Phase == InputPhase.Press)
        {
            if (!entity.TryGetComponent<BasicEnemy>(out var enemy) ||
            !entity.TryGetComponent<Position>(out var position) ||
            !entity.TryGetComponent<Velocity>(out var velocity) ||
            !entity.TryGetComponent<FacingDirection>(out var facingDirection) ||
            !entity.TryGetComponent<AnimationState>(out var animationState)
            )
                return;

            EnemyIndex++;

            if (EnemyIndex % 2 == 0)
            {
                position.Value = TestEnemyPosition;
                velocity.Value = TestEnemyVelocity;
                facingDirection.IsFacingLeft = true;
                enemy.Name = "goomba";
                animationState.CurrentState = "goomba";
            }
            else
            {
                position.Value = new Vector2(100, 100);
                velocity.Value = new Vector2(-50, 0);
                facingDirection.IsFacingLeft = true;
                enemy.Name = "koopa";
                animationState.CurrentState = "koopa";
            }
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
            !secondaryEntity.TryGetComponent<Position>(out var secondaryPosition)
        )
            return;

        if (direction.Equals(Direction.Left) || direction.Equals(Direction.Right))
        {
            //Only change direction if moving towards secondaryEntity
            if ((movingPosition.Value.X < secondaryPosition.Value.X && movingVelocity.Value.X > 0) || (movingPosition.Value.X > secondaryPosition.Value.X) && movingVelocity.Value.X < 0)
            {
                movingVelocity.Value.X *= -1;
                movingFacingDirection.IsFacingLeft = !movingFacingDirection.IsFacingLeft;
            }
        }
    }

    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<BasicEnemy>(out var enemy) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<FacingDirection>(out var facingDirection)
            )
                continue;


            if (position.Value.X <= 0 || position.Value.X >= 550) //TODO: remove magic numbers
            {
                movingVelocity.Value.X *= -1;
                movingFacingDirection.IsFacingLeft = !movingFacingDirection.IsFacingLeft;
            }
        }
    }
}