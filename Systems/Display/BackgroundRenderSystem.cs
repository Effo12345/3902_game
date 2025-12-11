namespace ECS.Systems;

public class BackgroundRenderSystem : AbstractSystem
{
    private const float PARALLAX_SCALE = 0.4f;

    private readonly SpriteBatch SpriteBatch;
    private readonly GraphicsManager Graphics;

    public BackgroundRenderSystem(GraphicsManager graphics)
    {
        SpriteBatch = graphics.SpriteBatch;
        Graphics = graphics;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        Matrix LatestCameraTransform = Graphics.CameraTransform;
        // Move the background more slowly than the foreground
        LatestCameraTransform.M41 *= PARALLAX_SCALE;
        // TODO: Calling this multiple times per update has a performance cost
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: LatestCameraTransform);

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<SpriteState>(out var sprite) ||
                !entity.TryGetComponent<SpriteTexture>(out var texture) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Rotation>(out var rotation) ||
                !entity.TryGetComponent<Scale>(out var scale) ||
                !entity.TryGetComponent<Tag>(out var tag) ||
                tag.Value != "background"
            )
                continue;

            SpriteBatch.Draw(
                texture.Texture,
                position.Value,
                sprite.SourceRect,
                sprite.Color,
                rotation.Value,
                sprite.Origin,
                scale.Value,
                SpriteEffects.None,
                0
            );
        }

        SpriteBatch.End();
    }
}
