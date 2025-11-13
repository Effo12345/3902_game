namespace ECS.Systems;

public class RenderSystem : AbstractSystem
{
    private const float SCROLL_WIDTH = 0.4f;

    private readonly SpriteBatch SpriteBatch;

    private float _midX; // determines where one wants to regard as middle of the screen
    private Entity _center;

    public RenderSystem(SpriteBatch spriteBatch)
    {
        SpriteBatch = spriteBatch;
        _center = null;
        _midX = spriteBatch.GraphicsDevice.Viewport.Width * SCROLL_WIDTH;
    }

    private static bool IsMario(Entity entity)
    {
        return entity.TryGetComponent<Position>(out var _)
            && entity.TryGetComponent<InputBindings>(out var _);
    }

    // TODO: Move camera adjustments to their own system
    private Vector2 PositionAdjustment()
    {
        foreach (var entity in Engine.GetEntities())
        {
            if (IsMario(entity))
            {
                _center = entity;
                break;
            }
        }

        if (_center == null)
        {
            // If the following loop finds nothing, use (0, 0)
            return new Vector2(_midX, 0);
        }
        else
        {
            Position position;
            _center.TryGetComponent<Position>(out position);
            // Currently, Y position is designed to be not adjusted
            return new Vector2(_midX - position.Value.X, 0);
        }
    }

    // TODO: Split this into its own system
    public override void Update(Engine engine, GameTime gameTime)
    {
        // Get the transformation matrix needed to offset by the camera amount...
        Vector2 cameraOffset = PositionAdjustment();
        Matrix cameraTransform = Matrix.CreateTranslation(cameraOffset.X, cameraOffset.Y, 0);

        // ...then publish it
        engine.PubSub.Publish(new CameraTransformEvent
        {
            Transform = cameraTransform
        });

        // PointClamp prevents scaled sprites from looking blurry
        SpriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: cameraTransform);

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<SpriteState>(out var sprite) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Rotation>(out var rotation) ||
                !entity.TryGetComponent<Scale>(out var scale)
            )
                continue;

            // Temperory for item switching
            if (entity.TryGetComponent<Visible>(out var visible) && !visible.Value)
                continue;

            var spriteEffects = SpriteEffects.None;
            if (entity.TryGetComponent<FacingDirection>(out var facing) && facing.IsFacingLeft)
                spriteEffects |= SpriteEffects.FlipHorizontally;
            if (entity.TryGetComponent<BasicEnemy>(out var enemy) && enemy.DeathType == DeathType.Flipped)
                spriteEffects |= SpriteEffects.FlipVertically;

            float depth = 0.5f;
            if (entity.TryGetComponent<RenderLayer>(out var layer))
                depth = MathHelper.Clamp(layer.Value, 0f, 1f);
            
            SpriteBatch.Draw(
                sprite.Texture,
                position.Value,
                sprite.SourceRect,
                sprite.Color,
                rotation.Value,
                sprite.Origin,
                scale.Value,
                spriteEffects,
                depth
                );

        }

        SpriteBatch.End();
    }
}
