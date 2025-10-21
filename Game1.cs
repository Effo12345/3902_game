namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Engine Engine = new Engine();

    // Temporary magic numbers for testing and sprint2-specific features
    // In future, will either be unneeded or moved to config files
    private readonly Vector2 TestEnemyPosition = new Vector2(100, 100);
    private readonly Vector2 TestEnemyVelocity = new Vector2(-50, 0);
    private readonly Vector2 TestWorldBounds = new Vector2(0.0f, 300);
    private readonly string[] TestItemAnimations = ["KoopaShell", "FireFlower", "SuperMushroom", "BoostMushroom", "SuperStar"];

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Engine.AddSystem(new GravitySystem());
        Engine.AddSystem(new GroundedSystem(TestWorldBounds));
        Engine.AddSystem(new InputSystem());
        Engine.AddSystem(new PlayerMotionSystem());
        Engine.AddSystem(new EnemyMovementSystem(TestEnemyPosition, TestEnemyVelocity));
        Engine.AddSystem(new FrictionSystem());
        Engine.AddSystem(new PhysicsSystem());
        Engine.AddSystem(new PlayerAnimationSystem());
        Engine.AddSystem(new FacingSystem());
        Engine.AddSystem(new AnimationSystem());
        Engine.AddSystem(new StartScreenSystem(this, Content, GraphicsDevice));

        // Temporary for item switching
        Engine.AddSystem(new DebrisSpawnSystem());
        Engine.AddSystem(new BlockBreakSystem());
        Engine.AddSystem(new BlockBumpSystem());                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
        Engine.AddSystem(new BlockPhysicsSystem());
        Engine.AddSystem(new BlockClickSystem());
        Engine.AddSystem(new ItemBlockSystem());
        Engine.AddSystem(new ItemSpawnSystem());
        Engine.AddSystem(new ItemChangeSystem(TestItemAnimations));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        // TODO: It's annoying to have these systems here by themselves
        // Engine.AddSystem(new MarioActionSystem(BasicMario, ItemSprite));
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