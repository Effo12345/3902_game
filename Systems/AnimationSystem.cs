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
        foreach (var entity in engine.GetEntities())
        {
            // if (!entity.TryGetComponent<AnimationConfig>(out var animComponent) ||
            //     !entity.TryGetComponent<AnimationState>(out var animState))
            //     continue;

        }
    }
}