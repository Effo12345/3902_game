namespace ECS.Systems;

public class RenderSystem : AbstractSystem
{
    private readonly SpriteBatch SpriteBatch;

    public RenderSystem(SpriteBatch spriteBatch)
    {
        SpriteBatch = spriteBatch;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        // PointClamp prevents scaled sprites from looking blurry
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

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
                spriteEffects = SpriteEffects.FlipHorizontally;

            SpriteBatch.Draw(
                sprite.Texture,
                position.Value,
                sprite.SourceRect,
                sprite.Color,
                rotation.Value,
                sprite.Origin,
                scale.Value,
                spriteEffects,
                0
                );
    
        }

        SpriteBatch.End();
    }
}