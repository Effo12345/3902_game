namespace ECS.Core;

public interface ISystem
{
    public bool Initialize(Engine engine);
    public void Update(Engine engine, GameTime gameTime);
}