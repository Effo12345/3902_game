namespace ECS.Systems;

public class TextRenderSystem : AbstractSystem
{
    private readonly SpriteBatch SpriteBatch;
    private readonly Assets Assets;

    public TextRenderSystem(SpriteBatch spriteBatch, Assets assets)
    {
        SpriteBatch = spriteBatch;
        Assets = assets;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        // TODO: Calling this multiple times per update has a performance cost
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<TextRender>(out var text) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Rotation>(out var rotation) ||
                !entity.TryGetComponent<Scale>(out var scale)
            )
                continue;

            SpriteBatch.DrawString(
                Assets.GetFont(text.Font),
                text.Text,
                position.Value,
                text.Color,
                rotation.Value,
                Vector2.Zero,
                scale.Value,
                SpriteEffects.None,
                0
            );
        }

        SpriteBatch.End();
    }
}
