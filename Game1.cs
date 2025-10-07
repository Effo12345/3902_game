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

    // Entities stored specifically to be passed in temporary system constructors
    // Will be removed after sprint 2
    private Entity BasicMario;
    private Texture2D ItemSprite;

    // Temporary game-scoped control tracking
    private bool resetLastFrame = false;

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

        SetupEntities();

        // TODO: It's annoying to have these systems here by themselves
        Engine.AddSystem(new MarioActionSystem(BasicMario, ItemSprite));
        Engine.AddSystem(new RenderSystem(_spriteBatch));
    }

    private void SetupEntities()
    {
        // Clear any entities that were previously instantiated
        Engine.GetEntities().Clear();

        // Intentionally get configs and content on each reset
        // This lets us test new configs without recompiling

        // Mario
        var marioSprites = Content.Load<Texture2D>("Sprites/mario");
        BasicMario = PlayerFactory.MakePlayer(Engine, marioSprites, "Content/Config/mario_spritesheet.json", "Content/Config/player_input.json");

        // Enemies
        Entity enemy = EnemyFactory.MakeEnemy(Engine, Content, "goomba", TestEnemyPosition, TestEnemyVelocity);

        // Items
        ItemSprite = Content.Load<Texture2D>("Sprites/items");
        Entity mobileItem = ItemFactory.MakeItem(
            Engine,
            new Vector2(650, 150),
            ItemSprite,
            "Content/Config/items_spritesheet.json",
            "Content/Config/item_input.json"
        );

        // Tiles
        var blockSprites = Content.Load<Texture2D>("Sprites/tiles");
        var tilesAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/tiles_spritesheet.json"));

        TileEntityConfigData tileEntityConfig = new TileEntityConfigData
        {
            Engine = Engine,
            Texture = blockSprites,
            Animation = tilesAnimationConfig
        };

        tileEntityConfig.Pos = new Vector2(600, 100);
        Entity bumpableBlock = TileEntityFactory.MakeBumpBlock(tileEntityConfig, "block");


        tileEntityConfig.Pos = new Vector2(640, 100);
        Entity breakableBlock = TileEntityFactory.MakeBreakBlock(tileEntityConfig, "block");

        // TODO: make a mapping between ItemKinds and animation state strings
        tileEntityConfig.Pos = new Vector2(680, 100);
        Entity itemBlock = TileEntityFactory.MakeItemBlock(tileEntityConfig, "unused_block", ItemKind.Mushroom, "red_mushroom", "used_block");


        tileEntityConfig.Pos = new Vector2(730, 100);
        Entity coin = TileEntityFactory.MakeCoin(tileEntityConfig);
    }

    protected override void Update(GameTime gameTime)
    {
        // Temporarily handle some broad inputs here while the InputSystem is being rewritten
        if (Keyboard.GetState().IsKeyDown(Keys.Q))
            Exit();

        if (Keyboard.GetState().IsKeyDown(Keys.R))
        {
            if (!resetLastFrame)
                SetupEntities();
            resetLastFrame = true;
        }
        else
        {
            resetLastFrame = false;
        }

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
