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
                velocity.Value.X *= -1;
                facingDirection.IsFacingLeft = !facingDirection.IsFacingLeft;
            }

        }

    }
}