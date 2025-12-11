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
            if (currentStatus.State != CONSTANTS.PLAYER_STATE.DEAD) { 
                if (currentStatus.isHovering) 
                {
                    animUpdate += CONSTANTS.ANIMATION_STATE.PLAYER_HOVER_SUFFIX;
                    currentStatus.isHovering = false;
                }
                else if (!isGrounded.Value)
                    animUpdate += CONSTANTS.ANIMATION_STATE.PLAYER_JUMP_SUFFIX;
                else if (Math.Abs(velocity.Value.X) < IDLE_THRESHOLD)
                    animUpdate += CONSTANTS.ANIMATION_STATE.PLAYER_IDLE_SUFFIX;
                else
                    animUpdate += CONSTANTS.ANIMATION_STATE.PLAYER_WALK_SUFFIX;
            }

            engine.PubSub.Publish(new AnimationChangeEvent
            {
                NewState = animUpdate,
                Entity = entity
            });
        }
    }
}