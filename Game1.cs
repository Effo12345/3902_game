namespace ECS;

public class Game1 : Game
{
    private Engine Engine = new Engine();
    private GameManager GameManager;
    private GraphicsManager GraphicsManager;

    public Game1()
    {
        GraphicsManager = new(this);
        GameManager = new(this, Engine);

        Content.RootDirectory = "Content";
    }

    protected override void Initialize()
    {
        Engine.AddSystem(new HitboxMovementSystem());
        Engine.AddSystem(new CollisionDetectionSystem());
        Engine.AddSystem(new PipeInteractionSystem());
        Engine.AddSystem(new InputSystem());
        Engine.AddSystem(new MarioCollisionSystem());
        Engine.AddSystem(new NonMarioCollisionSystem());
        Engine.AddSystem(new InvincibilitySystem());
        Engine.AddSystem(new StarSystem());
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
        Engine.AddSystem(new BlockBreakSystem());
        Engine.AddSystem(new CoinCleanupSystem());
        Engine.AddSystem(new EnemyCleanupSystem());
        Engine.AddSystem(new KoopaWakeupSystem());
        Engine.AddSystem(new MusicSystem());
        Engine.AddSystem(new LevelEndSystem(GraphicsManager));
        Engine.AddSystem(new MarioDefeatSystem(GameManager));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        var assets = AssetLoader.Load(Content, File.ReadAllText(CONSTANTS.FILE.ASSETS));
        EntityFactory.RegisterAssets(assets);
        LevelLoader.RegisterAssets(assets);
        SoundPlayer.RegisterAssets(assets);

        GameManager.Initialize(CONSTANTS.LEVEL.MARIO1_1);
        GraphicsManager.Initialize(assets);

        Engine.AddSystem(new PixelationSystem(GraphicsManager));
        Engine.AddSystem(new CameraSystem(GraphicsManager));
        Engine.AddSystem(new BackgroundRenderSystem(GraphicsManager));
        Engine.AddSystem(new RenderSystem(GraphicsManager));
        // Engine.AddSystem(new CollisionVisualizationSystem(GraphicsManager));
        // Engine.AddSystem(new VectorVisualizationSystem(GraphicsManager));
        Engine.AddSystem(new TextRenderSystem(GraphicsManager, assets));
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsManager.PreRender();

        // TODO: Add execution time and priority for systems
        Engine.Update(gameTime);

        GraphicsManager.PostRender();

        // Console.WriteLine($"Running slowly: {gameTime.IsRunningSlowly} Frame time: {gameTime.ElapsedGameTime.TotalMilliseconds}");

        base.Draw(gameTime);
    }
}
