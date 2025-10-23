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
        Engine.AddSystem(new CollisionDetectionSystem());
        Engine.AddSystem(new GroundedSystem(TestWorldBounds));
        Engine.AddSystem(new GravitySystem());
        Engine.AddSystem(new InputSystem());
        Engine.AddSystem(new PlayerMotionSystem());
        Engine.AddSystem(new EnemyMovementSystem( ));
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
        Entity goomba1 = EnemyFactory.MakeEnemy(Engine, Content, "goomba", new Vector2(300, 200), TestEnemyVelocity * -1, false);

        Entity goomba2 = EnemyFactory.MakeEnemy(Engine, Content, "koopa", new Vector2(500, 200), TestEnemyVelocity, true);

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

        tileEntityConfig.Pos = new Vector2(-44, 432);
        Entity Blocka = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(-44, 384);
        Entity Blockc = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(4, 432);
        Entity Blockb = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(52, 432);
        Entity Block0 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(100, 432);
        Entity Block1 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(148, 432);
        Entity Block2 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(196, 432);
        Entity Block3 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(242, 432);
        Entity Block4 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(290, 432);
        Entity Block5 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(338, 432);
        Entity Block6 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(386, 432);
        Entity Block7 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(434, 432);
        Entity Block8 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(482, 432);
        Entity Block9 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(530, 432);
        Entity Block10 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(578, 432);
        Entity Block11 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(626, 432);
        Entity Block12 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(674, 432);
        //Entity Block13 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(722, 432);
        //Entity Block14 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(770, 432);
        Entity Block15 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(770, 384);
        Entity Block18 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(818, 432);
        Entity Block16 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");

        tileEntityConfig.Pos = new Vector2(818, 384);
        Entity Block17 = TileEntityFactory.MakeBlock(tileEntityConfig, "block");


        var assets = AssetLoader.Load(Content, File.ReadAllText("Content/Config/assets.json"));
        EntityFactory.RegisterAssets(assets);

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