namespace ECS.Systems;

public class PlayerAnimationSystem : AbstractSystem
{
    private const float IDLE_THRESHOLD = 0.1f;

    // TODO: This needs replaced with a better design pattern
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            // TODO: Find a better way to discriminate players
            if (!entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<IsGrounded>(out var isGrounded) ||
                !entity.TryGetComponent<AnimationState>(out var animationState) ||
                !entity.TryGetComponent<Status>(out var currentStatus)
            )
                continue;

            string animUpdate;

            animUpdate = currentStatus.State;
            if (currentStatus.State != "dead") { 
                if (!isGrounded.Value)
                    animUpdate += "_jump";
                else if (Math.Abs(velocity.Value.X) < IDLE_THRESHOLD)
                    animUpdate += "_idle";
                else
                    animUpdate += "_walk";


            }

            //animUpdate = animationState.CurrentState.Split("_")[0] + "_" + animUpdate;

            engine.PubSub.Publish(new AnimationChangeEvent
            {
                NewState = animUpdate,
                Entity = entity
            });
        }
    }
}