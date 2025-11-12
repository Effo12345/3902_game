namespace ECS.Systems;

public class AnimationSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<AnimationChangeEvent>(HandleStateChange);

        return true;
    }

    private void HandleStateChange(IEvent evt)
    {
        var animationEvent = (AnimationChangeEvent)evt;

        if (!animationEvent.Entity.TryGetComponent<AnimationState>(out var animationState))
            return;

        if (animationState.CurrentState != animationEvent.NewState)
        {
            animationState.CurrentState = animationEvent.NewState;
            animationState.FrameIndex = 0;
            animationState.TimeInCurrentFrame = 0.0f;
        }
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<AnimationConfig>(out var animConfig) ||
                !entity.TryGetComponent<AnimationState>(out var animState) ||
                !entity.TryGetComponent<SpriteState>(out var spriteState)
            )
                continue;

            if (!animConfig.States.ContainsKey(animState.CurrentState))
                continue;

            var animDesc = animConfig.States[animState.CurrentState];

            animState.TimeInCurrentFrame += deltaTime;

            if (animState.TimeInCurrentFrame >= animDesc.FrameDuration)
            {
                animState.FrameIndex = (animState.FrameIndex + 1) % animDesc.Frames.Count;
                animState.TimeInCurrentFrame = 0;
                spriteState.SourceRect = animDesc.Frames[animState.FrameIndex];
            }
        }
    }
}