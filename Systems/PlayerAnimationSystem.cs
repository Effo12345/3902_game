
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
                !entity.TryGetComponent<Status>(out var currentStatus) ||
                !entity.HasComponent<InputConfig>()
            )
                continue;

            string animUpdate;

            if (Math.Abs(velocity.Value.X) < IDLE_THRESHOLD)
                animUpdate = "idle";
            else
                animUpdate = "walk";

            if (!isGrounded.Value)
                animUpdate = "jump";

            animUpdate = currentStatus.State + "_" + animUpdate;
            //animUpdate = animationState.CurrentState.Split("_")[0] + "_" + animUpdate;

            engine.PubSub.Publish(new AnimationChangeEvent
            {
                NewState = animUpdate,
                Entity = entity
            });
        }
    }
}