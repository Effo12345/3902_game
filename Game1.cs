namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Engine Engine = new Engine();
    private GameManager GameManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        // TODO: Move this somewhere else
        _graphics.PreferredBackBufferWidth = (int)CONSTANTS.RENDER.WindowSize.X;
        _graphics.PreferredBackBufferHeight = (int)CONSTANTS.RENDER.WindowSize.Y;

        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Engine.AddSystem(new HitboxMovementSystem());
        Engine.AddSystem(new CollisionDetectionSystem());
        Engine.AddSystem(new InputSystem());
        Engine.AddSystem(new MarioCollisionSystem());
        Engine.AddSystem(new NonMarioCollisionSystem());
        Engine.AddSystem(new InvincibilitySystem());
        Engine.AddSystem(new GravitySystem());
        Engine.AddSystem(new PlayerMotionSystem());
        Engine.AddSystem(new BoundedMotionSystem());
        Engine.AddSystem(new FrictionSystem());
        Engine.AddSystem(new PlayerAnimationSystem());
        Engine.AddSystem(new AnimationSystem());
        Engine.AddSystem(new GroundedSystem());
        Engine.AddSystem(new PhysicsSystem());
        Engine.AddSystem(new FacingSystem());
        Engine.AddSystem(new ItemSpawnSystem());
        Engine.AddSystem(new ItemBlockConsumptionSystem());
        Engine.AddSystem(new ItemRiseSystem());
        Engine.AddSystem(new BlockBumpMotionSystem());
        Engine.AddSystem(new EnemyCleanupSystem());
        Engine.AddSystem(new KoopaWakeupSystem());
        Engine.AddSystem(new MusicSystem());

        GameManager = new(this, Engine);

        Engine.AddSystem(new MarioDefeatSystem(GameManager));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var assets = AssetLoader.Load(Content, File.ReadAllText(CONSTANTS.FILE.ASSETS));
        EntityFactory.RegisterAssets(assets);
        LevelLoader.RegisterAssets(assets);
        SoundPlayer.RegisterAssets(assets);

        GameManager.Initialize(CONSTANTS.LEVEL.MARIO1_1);

        // TODO: It's annoying to have this system here by itself
        Engine.AddSystem(new BackgroundRenderSystem(_spriteBatch));
        Engine.AddSystem(new RenderSystem(_spriteBatch));
        // Engine.AddSystem(new CollisionVisualizationSystem(_graphics.GraphicsDevice, _spriteBatch));
        // Engine.AddSystem(new VectorVisualizationSystem(_graphics.GraphicsDevice, _spriteBatch));
        Engine.AddSystem(new TextRenderSystem(_spriteBatch, assets));
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add execution time and priority for systems
        Engine.Update(gameTime);

        // Console.WriteLine($"Running slowly: {gameTime.IsRunningSlowly} Frame time: {gameTime.ElapsedGameTime.TotalMilliseconds}");

        base.Draw(gameTime);
    }
}
