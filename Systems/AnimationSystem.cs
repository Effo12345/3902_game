namespace ECS.Systems;

public class AnimationSystem : AbstractSystem
{
    // public override bool Initialize(Engine engine)
    // {
    //     // Do whatever initialization behavior is needed here

    //     return base.Initialize(engine);
    // }

    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var e in engine.GetEntities())
        {
            if (!e.TryGetComponent<AnimationConfig>(out var cfg))
                continue;
            if (!e.TryGetComponent<AnimationState>(out var st))
                continue;
            if (!e.TryGetComponent<SpriteState>(out var sprite))
                continue;
            if (cfg.States == null || string.IsNullOrEmpty(st.CurrentState))
                continue;
            if (!cfg.States.TryGetValue(st.CurrentState, out var desc))
                continue;
            if (desc.Frames == null || desc.Frames.Count == 0)
                continue;

            st.TimeInCurrentFrame += dt;

            if (desc.FrameDuration > 0f && desc.Frames.Count > 1)
            {
                while (st.TimeInCurrentFrame >= desc.FrameDuration)
                {
                    st.TimeInCurrentFrame -= desc.FrameDuration;
                    st.FrameIndex++;
                    if (st.FrameIndex >= desc.Frames.Count) st.FrameIndex = 0;
                }
            }
            else
                st.FrameIndex = 0;
            sprite.SourceRect = desc.Frames[st.FrameIndex];
        }
    }
}