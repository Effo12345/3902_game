namespace ECS.Systems;

public class RenderSystem : AbstractSystem
{
    private readonly SpriteBatch SpriteBatch;
    private readonly GraphicsManager Graphics;

    public RenderSystem(GraphicsManager graphics)
    {
        Graphics = graphics;
        SpriteBatch = graphics.SpriteBatch;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        // PointClamp prevents scaled sprites from looking blurry
        SpriteBatch.Begin(SpriteSortMode.BackToFront, samplerState: SamplerState.PointClamp, transformMatrix: Graphics.CameraTransform);

        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<SpriteState>(out var sprite) ||
                !entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<Rotation>(out var rotation) ||
                !entity.TryGetComponent<Scale>(out var scale)
            )
                continue;

            // Handle cases where rendering isn't done or is by another system
            if (entity.TryGetComponent<Visible>(out var visible) && !visible.Value ||
                entity.TryGetComponent<Tag>(out var tag) && tag.Value == "background"
            )
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