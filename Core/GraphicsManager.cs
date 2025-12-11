namespace ECS.Components;

public class GraphicsManager
{
    private readonly GraphicsDeviceManager _graphics;
    public GraphicsDevice GraphicsDevice { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }

    public Matrix CameraTransform { get; set; }
    public Color BackgroundColor { get; set; } = CONSTANTS.TEMP.DEFAULT_BACKGROUND_COLOR;
    public float Pixelation { get; set; } = 0.0f;

    private Effect PixelationEffect;
    private RenderTarget2D SceneTarget;

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

        PixelationEffect = assets.GetShader(CONSTANTS.SHADER.PIXELATION_SHADER);

        SceneTarget = new RenderTarget2D(
            GraphicsDevice,
            GraphicsDevice.Viewport.Width,
            GraphicsDevice.Viewport.Height,
            false,
            SurfaceFormat.Color,
            DepthFormat.None
        );
    }

    /// <summary>
    /// Makes it so any spritebatch calls render into the SceneTarget buffer,
    /// rather than directly to the display buffer
    /// </summary>
    public void PreRender()
    {
        GraphicsDevice.SetRenderTarget(SceneTarget);
        GraphicsDevice.Clear(BackgroundColor);
    }

    /// <summary>
    /// Switch the render target back to the display buffer, then render into it
    /// the image produced inside SceneTarget.
    /// This lets us apply the pixelation effect uniformly across the entire screen
    /// </summary>
    public void PostRender()
    {
        GraphicsDevice.SetRenderTarget(null);
        PixelationEffect.Parameters[CONSTANTS.SHADER.PIXEL_SIZE_PROPERTY].SetValue(new Vector2(Pixelation, Pixelation));

        if (Pixelation > 0.0f)
            SpriteBatch.Begin(effect: PixelationEffect, samplerState: SamplerState.PointClamp);
        else
            SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        SpriteBatch.Draw(SceneTarget, Vector2.Zero, Color.White);
        SpriteBatch.End();
    }

    public void ClearPixelation()
    {
        Pixelation = 0.0f;
    }
}