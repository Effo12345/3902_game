
using System.Net;

namespace ECS.Systems;

public class EnemyMovementSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputEvent>(HandleInput);

        return true;
    }
    private void HandleInput(IEvent evt)
    {
        var inputEvent = (InputEvent)evt;
        var motionVector = inputEvent.MotionVector;
        var entity = inputEvent.Entity;

        if (!entity.TryGetComponent<BasicEnemy>(out var enemy) ||
            !entity.TryGetComponent<Position>(out var position) ||
            !entity.TryGetComponent<Velocity>(out var velocity) ||
            !entity.TryGetComponent<FacingDirection>(out var facingDirection) ||
            !entity.TryGetComponent<AnimationState>(out var animationState)
        )
            return;

        if (motionVector.X == 1.0)
        {
            enemy.Name = "goomba";
            position.Value = new Vector2(100, 100); //TODO: remove magic number
            velocity.Value = new Vector2(-50, 0); //TODO: remove magic number
            facingDirection.IsFacingLeft = true;
            animationState.CurrentState = "goomba";
        }
        if (motionVector.X == 2.0)
        {
            enemy.Name = "koopa";
            position.Value = new Vector2(100, 100); //TODO: remove magic number
            velocity.Value = new Vector2(-50, 0); //TODO: remove magic number
            facingDirection.IsFacingLeft = true;
            animationState.CurrentState = "koopa";
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


            if (position.Value.X <= 0 || position.Value.X >= 752) //TODO: remove magic numbers
            {
                velocity.Value.X *= -1;
                facingDirection.IsFacingLeft = !facingDirection.IsFacingLeft;
            }

        }

    }
}