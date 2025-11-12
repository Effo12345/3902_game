namespace ECS.Systems;

public class VectorVisualizationSystem : AbstractSystem
{
    private const int LINE_WEIGHT = 2;
    private const float VELOCITY_VECTOR_SCALE = 0.5f;
    private const float ACCELERATION_VECTOR_SCALE = 0.2f;

    private readonly GraphicsDevice Device;
    private readonly SpriteBatch SpriteBatch;
    private readonly Texture2D LineTexture;

    // TODO: This should get stored globally somehow
    private Matrix LatestCameraTransform;

    public VectorVisualizationSystem(GraphicsDevice graphics, SpriteBatch spriteBatch)
    {
        Device = graphics;
        SpriteBatch = spriteBatch;

        LineTexture = new(Device, 1, 1);
        LineTexture.SetData([Color.White]);
    }

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);

        engine.PubSub.CreateSubscription<CameraTransformEvent>(OnCameraTransform);

        return true;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        SpriteBatch.Begin(transformMatrix: LatestCameraTransform);

        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<SpriteState>(out var sprite)
            )
                continue;

            Vector2 vectorTail = position.Value + new Vector2(sprite.SourceRect.Width, sprite.SourceRect.Height) * 1.5f;

            // TODO: Only do this in debug mode
            if (entity.TryGetComponent<Velocity>(out var velocity))
            {
                Vector2 vectorHead = vectorTail + (velocity.Value * VELOCITY_VECTOR_SCALE);
                DrawLine(vectorTail, vectorHead, Color.Azure);
            }

            if (entity.TryGetComponent<Acceleration>(out var acceleration))
            {
                Vector2 vectorHead = vectorTail + (acceleration.Value * ACCELERATION_VECTOR_SCALE);
                DrawLine(vectorTail, vectorHead, Color.Coral);
            }
        }

        SpriteBatch.End();
    }

    private void DrawLine(Vector2 vectorTail, Vector2 vectorHead, Color color)
    {
        var distance = Vector2.Distance(vectorTail, vectorHead);
        var angle = (float)Math.Atan2(vectorHead.Y - vectorTail.Y, vectorHead.X - vectorTail.X);

        var origin = new Vector2(0f, 0.5f);
        var scale = new Vector2(distance, LINE_WEIGHT);

        SpriteBatch.Draw(
            LineTexture,
            vectorTail,
            null,
            color,
            angle,
            origin,
            scale,
            SpriteEffects.None,
            0
        );
    }

    private void OnCameraTransform(IEvent evt)
    {
        LatestCameraTransform = ((CameraTransformEvent)evt).Transform;
    }
}