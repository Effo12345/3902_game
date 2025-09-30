namespace ECS.Systems;

public class BlockAnimationSystem : AbstractSystem
{
    // public override bool Initialize(Engine engine)
    // {
    //     // Do whatever initialization behavior is needed here

    //     return base.Initialize(engine);
    // }

    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //loop all entities
        foreach (var e in engine.GetEntities())
        {
            //Check required components
            if (!e.TryGetComponent<AnimationConfig>(out var cfg))
                continue;
            if (!e.TryGetComponent<AnimationState>(out var st))
                continue;
            if (!e.TryGetComponent<SpriteState>(out var sprite))
                continue;
            //nothing to animate
            if (cfg.States == null || string.IsNullOrEmpty(st.CurrentState))
                continue;
            //state not defined
            if (!cfg.States.TryGetValue(st.CurrentState, out var desc))
                continue;
            //no frames
            if (desc.Frames == null || desc.Frames.Count == 0)
                continue;
            //advance time for current frame
            st.TimeInCurrentFrame += dt;
            //if animation has multiple frames and a frame duration
            if (desc.FrameDuration > 0f && desc.Frames.Count > 1)
            {
                //progress through frames when enough time passes
                while (st.TimeInCurrentFrame >= desc.FrameDuration)
                {
                    //subtract frame duration
                    st.TimeInCurrentFrame -= desc.FrameDuration;
                    //advance to next frame
                    st.FrameIndex++;
                    //loop back to first frame if past the last
                    if (st.FrameIndex >= desc.Frames.Count) st.FrameIndex = 0;
                }
            }
            else
                //if no animation always show the first frame
                st.FrameIndex = 0;
            //update sprite to current frame
            sprite.SourceRect = desc.Frames[st.FrameIndex];
        }
    }
}