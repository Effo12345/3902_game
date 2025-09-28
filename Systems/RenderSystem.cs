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

            //Case here to flip enemies if moving right... sorry this is terrible I just wanted to get it working
            if (entity.TryGetComponent<BasicEnemyMovement>(out var velocity) && velocity.Value.X > 0)
            {
                SpriteBatch.Draw(
                sprite.Texture,
                position.Value,
                sprite.SourceRect,
                sprite.Color,
                rotation.Value,
                sprite.Origin,
                scale.Value,
                SpriteEffects.FlipHorizontally,
                0
                );
            }
            else
            {

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
        }

        SpriteBatch.End();
    }
}