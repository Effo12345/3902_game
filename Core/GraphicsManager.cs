namespace ECS.Components;

public class GraphicsManager
{
    private readonly GraphicsDeviceManager _graphics;
    public GraphicsDevice GraphicsDevice { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }

    public Matrix CameraTransform { get; set; }
    public Color BackgroundColor { get; set; } = CONSTANTS.TEMP.DEFAULT_BACKGROUND_COLOR;

    public GraphicsManager(Game game)
    {
        _graphics = new GraphicsDeviceManager(game);
        _graphics.PreferredBackBufferWidth = (int)CONSTANTS.RENDER.WindowSize.X;
        _graphics.PreferredBackBufferHeight = (int)CONSTANTS.RENDER.WindowSize.Y;

        game.IsMouseVisible = true;
    }

    public void Initialize()
    {
        GraphicsDevice = _graphics.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);
    }
}