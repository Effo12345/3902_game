using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Engine Engine = new Engine();

    // Temporary for item switching
    private List<Entity> Items = new();

    private Vector2 TestenemyPosition = new Vector2(100, 100);
    private Vector2 TestenemyVelocity = new Vector2(-50, 0);

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Engine.AddSystem(new GravitySystem());
        Engine.AddSystem(new GroundedSystem(new Vector2(0.0f, 300)));
        Engine.AddSystem(new InputSystem());
        Engine.AddSystem(new PlayerMotionSystem());
        Engine.AddSystem(new EnemyMovementSystem(TestenemyPosition, TestenemyVelocity));
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
        Engine.AddSystem(new ItemChangeSystem(Items));

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        var marioSprites = Content.Load<Texture2D>("Sprites/mario");
        var marioAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/mario_spritesheet.json"));
        var marioControlConfig = InputDefinitionLoader.Load(File.ReadAllText("Content/Config/player_input.json"));

        var blockSprites = Content.Load<Texture2D>("Sprites/tiles");
        var tilesAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/tiles_spritesheet.json"));

        var itemSprite = Content.Load<Texture2D>("Sprites/items");
        var itemConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/items_spritesheet.json"));

        Entity koopaShell = Engine.CreateEntity();
        Entity fireflower = Engine.CreateEntity();
        Entity superMushroom = Engine.CreateEntity();
        Entity boostMushroom = Engine.CreateEntity();
        Entity superStar = Engine.CreateEntity();

        Items.Add(koopaShell);
        Items.Add(fireflower);
        Items.Add(superMushroom);
        Items.Add(boostMushroom);
        Items.Add(superStar);

        Entity basicMario = Engine.CreateEntity();

        Entity bumpableBlock = Engine.CreateEntity();
        Entity itemBlock = Engine.CreateEntity();
        Entity breakableBlock = Engine.CreateEntity();
        Entity coin = Engine.CreateEntity();
        Entity redMushroom = Engine.CreateEntity();
        Entity greenMushrrom = Engine.CreateEntity();
        Entity flower = Engine.CreateEntity();

        Entity enemy = EnemyFactory.MakeEnemy(Engine, Content, "goomba", TestenemyPosition, TestenemyVelocity);

        koopaShell.AddComponent(new Position { Value = new Vector2(200, 200) });
        koopaShell.AddComponent(new Velocity { Value = Vector2.Zero });
        koopaShell.AddComponent(new Rotation { Value = 0.0f });
        koopaShell.AddComponent(new Scale { Value = new Vector2(3, 3) });
        koopaShell.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["KoopaShell"].Frames[0], Color = Color.White });
        koopaShell.AddComponent(new AnimationConfig { States = itemConfig.States });
        koopaShell.AddComponent(new AnimationState { CurrentState = "KoopaShell" });
        koopaShell.AddComponent(new Visible { Value = true });

        fireflower.AddComponent(new Position { Value = new Vector2(200, 200) });
        fireflower.AddComponent(new Velocity { Value = Vector2.Zero });
        fireflower.AddComponent(new Rotation { Value = 0.0f });
        fireflower.AddComponent(new Scale { Value = new Vector2(3, 3) });
        fireflower.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["FireFlower"].Frames[0], Color = Color.White });
        fireflower.AddComponent(new AnimationConfig { States = itemConfig.States });
        fireflower.AddComponent(new AnimationState { CurrentState = "Fireflower" });
        fireflower.AddComponent(new Visible { Value = false });

        superMushroom.AddComponent(new Position { Value = new Vector2(200, 200) });
        superMushroom.AddComponent(new Velocity { Value = Vector2.Zero });
        superMushroom.AddComponent(new Rotation { Value = 0.0f });
        superMushroom.AddComponent(new Scale { Value = new Vector2(3, 3) });
        superMushroom.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["SuperMushroom"].Frames[0], Color = Color.White });
        superMushroom.AddComponent(new AnimationConfig { States = itemConfig.States });
        superMushroom.AddComponent(new AnimationState { CurrentState = "SuperMushroom" });
        superMushroom.AddComponent(new Visible { Value = false });

        boostMushroom.AddComponent(new Position { Value = new Vector2(200, 200) });
        boostMushroom.AddComponent(new Velocity { Value = Vector2.Zero });
        boostMushroom.AddComponent(new Rotation { Value = 0.0f });
        boostMushroom.AddComponent(new Scale { Value = new Vector2(3, 3) });
        boostMushroom.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["BoostMushroom"].Frames[0], Color = Color.White });
        boostMushroom.AddComponent(new AnimationConfig { States = itemConfig.States });
        boostMushroom.AddComponent(new AnimationState { CurrentState = "BoostMushroom" });
        boostMushroom.AddComponent(new Visible { Value = false });

        superStar.AddComponent(new Position { Value = new Vector2(200, 200) });
        superStar.AddComponent(new Velocity { Value = Vector2.Zero });
        superStar.AddComponent(new Rotation { Value = 0.0f });
        superStar.AddComponent(new Scale { Value = new Vector2(3, 3) });
        superStar.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["SuperStar"].Frames[0], Color = Color.White });
        superStar.AddComponent(new AnimationConfig { States = itemConfig.States });
        superStar.AddComponent(new AnimationState { CurrentState = "SuperStar" });
        superStar.AddComponent(new Visible { Value = false });

        bumpableBlock.AddComponent(new Position { Value = new Vector2(100, 150) });
        bumpableBlock.AddComponent(new Rotation { Value = 0f });
        bumpableBlock.AddComponent(new Scale { Value = new Vector2(2, 2) });
        bumpableBlock.AddComponent(new Bump());
        bumpableBlock.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["block"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        breakableBlock.AddComponent(new Position { Value = new Vector2(150, 150) });
        breakableBlock.AddComponent(new Rotation { Value = 0f });
        breakableBlock.AddComponent(new Scale { Value = new Vector2(2, 2) });
        breakableBlock.AddComponent(new Break());
        breakableBlock.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["block"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        itemBlock.AddComponent(new Position { Value = new Vector2(200, 150) });
        itemBlock.AddComponent(new Rotation { Value = 0f });
        itemBlock.AddComponent(new Scale { Value = new Vector2(2, 2) });
        itemBlock.AddComponent(new Bump());
        itemBlock.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["unused_block"].Frames[0], // show "?"
            Origin = Vector2.Zero,
            Color = Color.White
        });
        itemBlock.AddComponent(new ItemBlock
        {
            Kind = ItemKind.Mushroom,
            ItemTexture = blockSprites,
            ItemRect = tilesAnimationConfig.States["red_mushroom"].Frames[0],
            UsedBlockRect = tilesAnimationConfig.States["used_block"].Frames[0],
            Used = false
        });

        var coinFrames = tilesAnimationConfig.States["coin"].Frames;
        var coinDur = tilesAnimationConfig.States["coin"].FrameDuration;
        coin.AddComponent(new Position { Value = new Vector2(250, 150) });
        coin.AddComponent(new Rotation { Value = 0f });
        coin.AddComponent(new Scale { Value = new Vector2(2, 2) });
        coin.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = coinFrames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });
        coin.AddComponent(new AnimationConfig
        {
            States = new Dictionary<string, AnimationDescription>
            {
                { "coin", new AnimationDescription { Frames = coinFrames, FrameDuration = coinDur } }
            }
        });
        coin.AddComponent(new AnimationState
        {
            CurrentState = "coin",
            FrameIndex = 0,
            TimeInCurrentFrame = 0f
        });

        redMushroom.AddComponent(new Position { Value = new Vector2(300, 150) });
        redMushroom.AddComponent(new Rotation { Value = 0f });
        redMushroom.AddComponent(new Scale { Value = new Vector2(2, 2) });
        redMushroom.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["red_mushroom"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        greenMushrrom.AddComponent(new Position { Value = new Vector2(350, 150) });
        greenMushrrom.AddComponent(new Rotation { Value = 0f });
        greenMushrrom.AddComponent(new Scale { Value = new Vector2(2, 2) });
        greenMushrrom.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["green_mushroom"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        flower.AddComponent(new Position { Value = new Vector2(400, 150) });
        flower.AddComponent(new Rotation { Value = 0f });
        flower.AddComponent(new Scale { Value = new Vector2(2, 2) });
        flower.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["flower"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        basicMario.AddComponent(new Position { Value = new Vector2(20, 20) });
        basicMario.AddComponent(new Velocity { Value = Vector2.Zero });
        basicMario.AddComponent(new Rotation { Value = 0.0f });
        basicMario.AddComponent(new Scale { Value = new Vector2(3, 3) });
        basicMario.AddComponent(new SpriteState { Texture = marioSprites, Color = Color.White, Origin = new Vector2(0, 0) });
        basicMario.AddComponent(new Mass { Value = 5 });
        basicMario.AddComponent(new Acceleration());
        basicMario.AddComponent(new IsGrounded());
        basicMario.AddComponent(marioControlConfig);
        basicMario.AddComponent(new Friction { Value = 4.0f });
        basicMario.AddComponent(new VelocityCap { Value = new Vector2(0.2f, 10.0f) });
        basicMario.AddComponent(marioAnimationConfig);
        basicMario.AddComponent(new AnimationState { CurrentState = "small_idle" });
        basicMario.AddComponent(new FacingDirection());
        basicMario.AddComponent(new Status { State = "small" });

        // TODO: It's annoying to have these systems here by themselves
        Engine.AddSystem(new MarioActionSystem(basicMario, itemSprite));
        Engine.AddSystem(new RenderSystem(_spriteBatch));
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
            
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
