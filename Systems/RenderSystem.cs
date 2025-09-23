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
        SpriteBatch.Begin();

        foreach (var entity in Engine.GetEntities()) {}
    }
}