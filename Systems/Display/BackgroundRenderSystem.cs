namespace ECS.Systems;

public class BackgroundRenderSystem : AbstractSystem
{
    private const float PARALLAX_SCALE = 0.4f;

    private readonly SpriteBatch SpriteBatch;

    private Matrix LatestCameraTransform;

    public BackgroundRenderSystem(SpriteBatch spriteBatch)
    {
        SpriteBatch = spriteBatch;
    }

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<CameraTransformEvent>(OnCameraTransform);

        return true;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        // Move the background more slowly than the foreground
        LatestCameraTransform.M41 *= PARALLAX_SCALE;
        // TODO: Calling this multiple times per update has a performance cost
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: LatestCameraTransform);

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<SpriteState>(out var sprite) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Rotation>(out var rotation) ||
                !entity.TryGetComponent<Scale>(out var scale) ||
                !entity.TryGetComponent<Tag>(out var tag) ||
                tag.Value != "background"
            )
                continue;

            SpriteBatch.Draw(
                sprite.Texture,
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

    private void OnCameraTransform(IEvent evt)
    {
        LatestCameraTransform = ((CameraTransformEvent)evt).Transform;
    }
}
