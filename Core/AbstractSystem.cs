namespace ECS.Core;

public abstract class AbstractSystem
{
    // Provide persistent engine access in case another method needs it
    protected Engine Engine { get; set; }

    public virtual bool Initialize(Engine engine)
    {
        Engine = engine;
        return true;
    }

    public abstract void Update(Engine engine, GameTime gameTime);
}