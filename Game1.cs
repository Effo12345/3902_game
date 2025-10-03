using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Engine Engine = new Engine();
    private Entity BlueShell, FireFlower, SuperMushroom, BoostMushroom, Fireball, SuperStar;

    private Entity[] Items = new Entity[6];
    private Entity BumpableBlock;
    private Entity BreakableBlock;
    private Entity ItemBlock;
    private Entity Coin;
    private Entity GreenMushroom;
    private Entity RedMushroom;
    private Entity Flower;
    private Entity Enemy;
    private Entity TestEntity;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {

        BlueShell = Engine.CreateEntity();
        FireFlower = Engine.CreateEntity();
        Fireball = Engine.CreateEntity();
        SuperMushroom = Engine.CreateEntity();
        BoostMushroom = Engine.CreateEntity();
        SuperStar = Engine.CreateEntity();

        Items[0] = BlueShell;
        Items[1] = FireFlower;
        Items[2] = Fireball;
        Items[3] = SuperMushroom;
        Items[4] = BoostMushroom;
        Items[5] = SuperStar;

        // Temporary for item switching
        Engine.AddSystem(new ItemChangeSystem(Items));

        // TODO: Add your initialization logic here
        TestEntity = Engine.CreateEntity();
        Enemy = Engine.CreateEntity();

        Engine.AddSystem(new GravitySystem());
        Engine.AddSystem(new GroundedSystem(new Vector2(0.0f, 300)));
        Engine.AddSystem(new InputSystem());
        Engine.AddSystem(new PlayerMotionSystem());
        Engine.AddSystem(new FrictionSystem());
        Engine.AddSystem(new PhysicsSystem());
        Engine.AddSystem(new PlayerAnimationSystem());
        Engine.AddSystem(new FacingSystem());
        Engine.AddSystem(new AnimationSystem());
        Engine.AddSystem(new EnemyMovementSystem());

        BumpableBlock = Engine.CreateEntity();
        ItemBlock = Engine.CreateEntity();
        BreakableBlock = Engine.CreateEntity();
        Coin = Engine.CreateEntity();
        RedMushroom = Engine.CreateEntity();
        GreenMushroom = Engine.CreateEntity();
        Flower = Engine.CreateEntity();

        Engine.AddSystem(new BlockBreakSystem());
        Engine.AddSystem(new BlockBumpSystem());
        Engine.AddSystem(new BlockPhysicsSystem());
        Engine.AddSystem(new BlockClickSystem());
        Engine.AddSystem(new ItemBlockSystem());
        Engine.AddSystem(new ItemSpawnSystem());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        Engine.AddSystem(new RenderSystem(_spriteBatch));

        var marioSprites = Content.Load<Texture2D>("Sprites/mario");
        var marioAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/mario_spritesheet.json"));

        var blockSprites = Content.Load<Texture2D>("Sprites/tiles");
        var tilesAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/tiles_spritesheet.json"));

        var blueshellSprite = Content.Load<Texture2D>("Sprites/blueshell");
        var attacksSprite = Content.Load<Texture2D>("Sprites/attacks");
        var itemSprite = Content.Load<Texture2D>("Sprites/items");
        var bg_cloudSprite = Content.Load<Texture2D>("Sprites/bg_clouds");
        var bg_mountainSprite = Content.Load<Texture2D>("Sprites/bg_mountains");
        var bg_treeSprite = Content.Load<Texture2D>("Sprites/bg_trees");

        var attacksConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/attacks.json"));
        var itemConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/items.json"));
        var backgroundConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/backgrounds.json"));

        BlueShell.AddComponent(new Position { Value = new Vector2(200, 200) });
        BlueShell.AddComponent(new Velocity { Value = Vector2.Zero });
        BlueShell.AddComponent(new Rotation { Value = 0.0f });
        BlueShell.AddComponent(new Scale { Value = new Vector2(3, 3) });
        BlueShell.AddComponent(new SpriteState { Texture = blueshellSprite, SourceRect = itemConfig.States["Blueshell"].Frames[0], Color = Color.White });
        BlueShell.AddComponent(new Visible { Value = false });

        Fireball.AddComponent(new Position { Value = new Vector2(200, 200) });
        Fireball.AddComponent(new Velocity { Value = Vector2.Zero });
        Fireball.AddComponent(new Rotation { Value = 0.0f });
        Fireball.AddComponent(new Scale { Value = new Vector2(3, 3) });
        Fireball.AddComponent(new SpriteState { Texture = attacksSprite, SourceRect = attacksConfig.States["Fireball"].Frames[0], Color = Color.White });
        Fireball.AddComponent(new AnimationConfig { States = attacksConfig.States });
        Fireball.AddComponent(new AnimationState { CurrentState = "Fireball", FrameIndex = 0, TimeInCurrentFrame = attacksConfig.States["Fireball"].FrameDuration });
        Fireball.AddComponent(new Visible { Value = false });

        FireFlower.AddComponent(new Position { Value = new Vector2(200, 200) });
        FireFlower.AddComponent(new Velocity { Value = Vector2.Zero });
        FireFlower.AddComponent(new Rotation { Value = 0.0f });
        FireFlower.AddComponent(new Scale { Value = new Vector2(3, 3) });
        FireFlower.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["FireFlower"].Frames[0], Color = Color.White });
        FireFlower.AddComponent(new AnimationConfig { States = itemConfig.States });
        FireFlower.AddComponent(new AnimationState { CurrentState = "FireFlower", FrameIndex = 0, TimeInCurrentFrame = itemConfig.States["FireFlower"].FrameDuration });
        FireFlower.AddComponent(new Visible { Value = false });

        SuperMushroom.AddComponent(new Position { Value = new Vector2(200, 200) });
        SuperMushroom.AddComponent(new Velocity { Value = Vector2.Zero });
        SuperMushroom.AddComponent(new Rotation { Value = 0.0f });
        SuperMushroom.AddComponent(new Scale { Value = new Vector2(3, 3) });
        SuperMushroom.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["SuperMushroom"].Frames[0], Color = Color.White });
        SuperMushroom.AddComponent(new AnimationConfig { States = itemConfig.States });
        SuperMushroom.AddComponent(new AnimationState { CurrentState = "SuperMushroom", FrameIndex = 0, TimeInCurrentFrame = itemConfig.States["SuperMushroom"].FrameDuration });
        SuperMushroom.AddComponent(new Visible { Value = false });

        BoostMushroom.AddComponent(new Position { Value = new Vector2(200, 200) });
        BoostMushroom.AddComponent(new Velocity { Value = Vector2.Zero });
        BoostMushroom.AddComponent(new Rotation { Value = 0.0f });
        BoostMushroom.AddComponent(new Scale { Value = new Vector2(3, 3) });
        BoostMushroom.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["BoostMushroom"].Frames[0], Color = Color.White });
        BoostMushroom.AddComponent(new AnimationConfig { States = itemConfig.States });
        BoostMushroom.AddComponent(new AnimationState { CurrentState = "BoostMushroom", FrameIndex = 0, TimeInCurrentFrame = itemConfig.States["BoostMushroom"].FrameDuration });
        BoostMushroom.AddComponent(new Visible { Value = false });

        SuperStar.AddComponent(new Position { Value = new Vector2(200, 200) });
        SuperStar.AddComponent(new Velocity { Value = Vector2.Zero });
        SuperStar.AddComponent(new Rotation { Value = 0.0f });
        SuperStar.AddComponent(new Scale { Value = new Vector2(3, 3) });
        SuperStar.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["SuperStar"].Frames[0], Color = Color.White });
        SuperStar.AddComponent(new AnimationConfig { States = itemConfig.States });
        SuperStar.AddComponent(new AnimationState { CurrentState = "SuperStar", FrameIndex = 0, TimeInCurrentFrame = itemConfig.States["SuperStar"].FrameDuration });
        SuperStar.AddComponent(new Visible { Value = false });
        var marioControlConfig = InputDefinitionLoader.Load(File.ReadAllText("Content/Config/player_input.json"));

        BumpableBlock.AddComponent(new Position { Value = new Vector2(100, 150) });
        BumpableBlock.AddComponent(new Rotation { Value = 0f });
        BumpableBlock.AddComponent(new Scale { Value = new Vector2(2, 2) });
        BumpableBlock.AddComponent(new Bump());
        BumpableBlock.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["block"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        BreakableBlock.AddComponent(new Position { Value = new Vector2(150, 150) });
        BreakableBlock.AddComponent(new Rotation { Value = 0f });
        BreakableBlock.AddComponent(new Scale { Value = new Vector2(2, 2) });
        BreakableBlock.AddComponent(new Break());
        BreakableBlock.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["block"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        ItemBlock.AddComponent(new Position { Value = new Vector2(200, 150) });
        ItemBlock.AddComponent(new Rotation { Value = 0f });
        ItemBlock.AddComponent(new Scale { Value = new Vector2(2, 2) });
        ItemBlock.AddComponent(new Bump());
        ItemBlock.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["unused_block"].Frames[0], // show "?"
            Origin = Vector2.Zero,
            Color = Color.White
        });
        ItemBlock.AddComponent(new ItemBlock
        {
            Kind = ItemKind.Mushroom,
            ItemTexture = blockSprites,
            ItemRect = tilesAnimationConfig.States["red_mushroom"].Frames[0],
            UsedBlockRect = tilesAnimationConfig.States["used_block"].Frames[0],
            Used = false
        });

        var coinFrames = tilesAnimationConfig.States["coin"].Frames;
        var coinDur = tilesAnimationConfig.States["coin"].FrameDuration;
        Coin.AddComponent(new Position { Value = new Vector2(250, 150) });
        Coin.AddComponent(new Rotation { Value = 0f });
        Coin.AddComponent(new Scale { Value = new Vector2(2, 2) });
        Coin.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = coinFrames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });
        Coin.AddComponent(new AnimationConfig
        {
            States = new Dictionary<string, AnimationDescription>
            {
                { "coin", new AnimationDescription { Frames = coinFrames, FrameDuration = coinDur } }
            }
        });
        Coin.AddComponent(new AnimationState
        {
            CurrentState = "coin",
            FrameIndex = 0,
            TimeInCurrentFrame = 0f
        });

        RedMushroom.AddComponent(new Position { Value = new Vector2(300, 150) });
        RedMushroom.AddComponent(new Rotation { Value = 0f });
        RedMushroom.AddComponent(new Scale { Value = new Vector2(2, 2) });
        RedMushroom.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["red_mushroom"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        GreenMushroom.AddComponent(new Position { Value = new Vector2(350, 150) });
        GreenMushroom.AddComponent(new Rotation { Value = 0f });
        GreenMushroom.AddComponent(new Scale { Value = new Vector2(2, 2) });
        GreenMushroom.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["green_mushroom"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        Flower.AddComponent(new Position { Value = new Vector2(400, 150) });
        Flower.AddComponent(new Rotation { Value = 0f });
        Flower.AddComponent(new Scale { Value = new Vector2(2, 2) });
        Flower.AddComponent(new SpriteState
        {
            Texture = blockSprites,
            SourceRect = tilesAnimationConfig.States["flower"].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });
        TestEntity.AddComponent(new Position { Value = new Vector2(20, 20) });
        TestEntity.AddComponent(new Velocity { Value = Vector2.Zero });
        TestEntity.AddComponent(new Rotation { Value = 0.0f });
        TestEntity.AddComponent(new Scale { Value = new Vector2(3, 3) });
        TestEntity.AddComponent(new SpriteState { Texture = marioSprites, Color = Color.White, Origin = new Vector2(0, 0) });
        TestEntity.AddComponent(new Mass { Value = 5 });
        TestEntity.AddComponent(new Acceleration());
        TestEntity.AddComponent(new IsGrounded());
        TestEntity.AddComponent(marioControlConfig);
        TestEntity.AddComponent(new Friction { Value = 4.0f });
        TestEntity.AddComponent(new VelocityCap { Value = new Vector2(0.2f, 10.0f) });
        TestEntity.AddComponent(marioAnimationConfig);
        TestEntity.AddComponent(new AnimationState { CurrentState = "small_idle" });
        TestEntity.AddComponent(new FacingDirection());
        
        //Load in Enemies
        var enemySprites = Content.Load<Texture2D>("Sprites/enemies");
        var enemyAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/enemy_spritesheet.json"));

        Enemy = EnemyFactory.makeEnemy(Engine, Content, "goomba", new Vector2(100, 100));
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

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
