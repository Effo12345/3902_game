namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Engine Engine = new();
    private Entity BlueShell, FireFlower, SuperMushroom, BoostMushroom, Fireball, SuperStar;

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

        Engine.AddSystem(new DemoInputSystem());
        Engine.AddSystem(new DemoMovementSystem());
        Engine.AddSystem(new AnimationSystem());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        Engine.AddSystem(new RenderSystem(_spriteBatch));

        //var marioSprites = Content.Load<Texture2D>("Sprites/mario");
        //var marioAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/mario_spritesheet.json"));

        var blueshellSprite = Content.Load<Texture2D>("Sprites/blueshell");
        var blueshellConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/blueshell.json"));

        BlueShell.AddComponent(new Position { Value = new Vector2(20, 20) });
        BlueShell.AddComponent(new Velocity { Value = Vector2.Zero });
        BlueShell.AddComponent(new Rotation { Value = 0.0f });
        BlueShell.AddComponent(new Scale { Value = new Vector2(3, 3) });
        BlueShell.AddComponent(new SpriteState { Texture = blueshellSprite, SourceRect = blueshellConfig.States["Blueshell"].Frames[0], Color = Color.White });

        var attacksSprite = Content.Load<Texture2D>("Sprites/attacks");
        var attacksConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/attacks.json"));

        Fireball.AddComponent(new Position { Value = new Vector2(50, 20) });
        Fireball.AddComponent(new Velocity { Value = Vector2.Zero });
        Fireball.AddComponent(new Rotation { Value = 0.0f });
        Fireball.AddComponent(new Scale { Value = new Vector2(3, 3) });
        Fireball.AddComponent(new SpriteState { Texture = attacksSprite, SourceRect = attacksConfig.States["Fireball"].Frames[0], Color = Color.White });
        Fireball.AddComponent(new AnimationConfig { States = attacksConfig.States });
        Fireball.AddComponent(new AnimationState { CurrentState = "Fireball", FrameIndex = 0, TimeInCurrentFrame = attacksConfig.States["Fireball"].FrameDuration });

        var itemSprite = Content.Load<Texture2D>("Sprites/items");
        var itemConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/powerups.json"));

        FireFlower.AddComponent(new Position { Value = new Vector2(120, 20) });
        FireFlower.AddComponent(new Velocity { Value = Vector2.Zero });
        FireFlower.AddComponent(new Rotation { Value = 0.0f });
        FireFlower.AddComponent(new Scale { Value = new Vector2(3, 3) });
        FireFlower.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["FireFlower"].Frames[0], Color = Color.White });
        FireFlower.AddComponent(new AnimationConfig { States = itemConfig.States });
        FireFlower.AddComponent(new AnimationState { CurrentState = "FireFlower", FrameIndex = 0, TimeInCurrentFrame = itemConfig.States["FireFlower"].FrameDuration });

        SuperMushroom.AddComponent(new Position { Value = new Vector2(180, 20) });
        SuperMushroom.AddComponent(new Velocity { Value = Vector2.Zero });
        SuperMushroom.AddComponent(new Rotation { Value = 0.0f });
        SuperMushroom.AddComponent(new Scale { Value = new Vector2(3, 3) });
        SuperMushroom.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["SuperMushroom"].Frames[0], Color = Color.White });
        SuperMushroom.AddComponent(new AnimationConfig { States = itemConfig.States });
        SuperMushroom.AddComponent(new AnimationState { CurrentState = "SuperMushroom", FrameIndex = 0, TimeInCurrentFrame = itemConfig.States["SuperMushroom"].FrameDuration });

        BoostMushroom.AddComponent(new Position { Value = new Vector2(240, 20) });
        BoostMushroom.AddComponent(new Velocity { Value = Vector2.Zero });
        BoostMushroom.AddComponent(new Rotation { Value = 0.0f });
        BoostMushroom.AddComponent(new Scale { Value = new Vector2(3, 3) });
        BoostMushroom.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["BoostMushroom"].Frames[0], Color = Color.White });
        BoostMushroom.AddComponent(new AnimationConfig { States = itemConfig.States });
        BoostMushroom.AddComponent(new AnimationState { CurrentState = "BoostMushroom", FrameIndex = 0, TimeInCurrentFrame = itemConfig.States["BoostMushroom"].FrameDuration });


        SuperStar.AddComponent(new Position { Value = new Vector2(300, 20) });
        SuperStar.AddComponent(new Velocity { Value = Vector2.Zero });
        SuperStar.AddComponent(new Rotation { Value = 0.0f });
        SuperStar.AddComponent(new Scale { Value = new Vector2(3, 3) });
        SuperStar.AddComponent(new SpriteState { Texture = itemSprite, SourceRect = itemConfig.States["SuperStar"].Frames[0], Color = Color.White });
        SuperStar.AddComponent(new AnimationConfig { States = itemConfig.States });
        SuperStar.AddComponent(new AnimationState { CurrentState = "SuperStar", FrameIndex = 0, TimeInCurrentFrame = itemConfig.States["SuperStar"].FrameDuration });
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
