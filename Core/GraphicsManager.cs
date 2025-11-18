namespace ECS.Components;

public class GraphicsManager
{
    private readonly GraphicsDeviceManager _graphics;
    public GraphicsDevice GraphicsDevice { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }

    public Matrix CameraTransform { get; set; }
    public Color BackgroundColor { get; set; } = CONSTANTS.TEMP.DEFAULT_BACKGROUND_COLOR;

    private Effect PixelationEffect;
    private RenderTarget2D SceneTarget;

    private float Pixelation = 0.05f;

    public GraphicsManager(Game game)
    {
        _graphics = new GraphicsDeviceManager(game);
        _graphics.PreferredBackBufferWidth = (int)CONSTANTS.RENDER.WindowSize.X;
        _graphics.PreferredBackBufferHeight = (int)CONSTANTS.RENDER.WindowSize.Y;

        game.IsMouseVisible = true;
    }

    public void Initialize(Assets assets)
    {
        GraphicsDevice = _graphics.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        PixelationEffect = assets.GetShader("PixelationShader");

        SceneTarget = new RenderTarget2D(
            GraphicsDevice,
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height,
            false,
            SurfaceFormat.Color,
            DepthFormat.None
        );
    }

    public void PreRender()
    {
        GraphicsDevice.SetRenderTarget(SceneTarget);

        GraphicsDevice.Clear(BackgroundColor);
    }

    public void PostRender()
    {
        GraphicsDevice.SetRenderTarget(null);
        PixelationEffect.Parameters["PixelSize"].SetValue(new Vector2(Pixelation, Pixelation));

        if (Pixelation > 0.0f)
        {
            SpriteBatch.Begin(effect: PixelationEffect, samplerState: SamplerState.PointClamp);
            Pixelation -= 0.0015f;
        }
        else {
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        }

        SpriteBatch.Draw(SceneTarget, Vector2.Zero, Color.White);
        SpriteBatch.End();
    }
}