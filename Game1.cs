namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Engine Engine = new();
    private Entity BumpableBlock;
    private Entity BreakableBlock;
    private Entity ItemBlock;
    private Entity Coin;
    private Entity GreenMushroom;
    private Entity RedMushroom;
    private Entity Flower;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
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
        Engine.AddSystem(new AnimationSystem());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        Engine.AddSystem(new RenderSystem(_spriteBatch));

        var blockSprites = Content.Load<Texture2D>("Sprites/tiles");
        var tilesAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/tiles_spritesheet.json"));

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
    }
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

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
