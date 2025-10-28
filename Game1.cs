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
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Engine.AddSystem(new CollisionDetectionSystem());
        Engine.AddSystem(new InputSystem());
        Engine.AddSystem(new MarioCollisionSystem());
        Engine.AddSystem(new EnemyCollisionSystem());
        Engine.AddSystem(new GravitySystem());
        Engine.AddSystem(new PlayerMotionSystem());
        Engine.AddSystem(new FrictionSystem());
        Engine.AddSystem(new PlayerAnimationSystem());
        Engine.AddSystem(new AnimationSystem());
        Engine.AddSystem(new GroundedSystem());
        Engine.AddSystem(new PhysicsSystem());
        Engine.AddSystem(new FacingSystem());

        GameManager = new(this, Engine);

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var assets = AssetLoader.Load(Content, File.ReadAllText("Content/Config/assets.json"));
        EntityFactory.RegisterAssets(assets);
        LevelLoader.RegisterAssets(assets);

        GameManager.Initialize("Mario1_1");

        // TODO: It's annoying to have these systems here by themselves
        Engine.AddSystem(new RenderSystem(_spriteBatch));
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

        base.Draw(gameTime);
    }
}
