namespace ECS.Systems;

public class PixelationSystem : AbstractSystem
{
    private readonly GraphicsManager Graphics;

    public PixelationSystem(GraphicsManager graphics)
    {
        Graphics = graphics;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<PixelationIntro>(out var anim))
                continue;

            if (anim.Value > 0.0)
            {
                Graphics.Pixelation = anim.Value;
                anim.Value -= anim.StepSize;
            }
        }
    }
}