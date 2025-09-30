namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Engine Engine = new();
    private Entity BlueShell, FireFlower, SuperMushroom, BoostMushroom, Fireball, SuperStar;

    private Entity[] Items = new Entity[6];
    private Entity TestEntity;
    private Entity Goomba;
    private Entity Koopa;

    private Vector2 enemyVelocity = new Vector2(-100, 0); //(Probably temporary) default velocity for enemies to set their velocity in BasicEnemyMovement as.

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
        Goomba = Engine.CreateEntity();
        Koopa = Engine.CreateEntity();

        Engine.AddSystem(new GravitySystem());
        Engine.AddSystem(new GroundedSystem(new Vector2(0.0f, 300)));
        Engine.AddSystem(new InputSystem());
        Engine.AddSystem(new PlayerMotionSystem());
        Engine.AddSystem(new FrictionSystem());
        Engine.AddSystem(new PhysicsSystem());
        Engine.AddSystem(new PlayerAnimationSystem());
        Engine.AddSystem(new FacingSystem());
        Engine.AddSystem(new SpriteSwitchingSystem());
        Engine.AddSystem(new EnemyMovementSystem());
        Engine.AddSystem(new AnimationSystem());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        Engine.AddSystem(new RenderSystem(_spriteBatch));

        var marioSprites = Content.Load<Texture2D>("Sprites/mario");
        var marioAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/mario_spritesheet.json"));

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

        //Load in Goomba
        var goombaSprites = Content.Load<Texture2D>("Sprites/goombas");
        var goombaAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/goomba_spritesheet.json"));

        Goomba.AddComponent(new Position { Value = new Vector2(100, 100) });
        Goomba.AddComponent(new BasicEnemy { Name = "Goomba", Value = enemyVelocity });
        Goomba.AddComponent(new Rotation { Value = 0.0f });
        Goomba.AddComponent(new Scale { Value = new Vector2(3, 3) });
        Goomba.AddComponent(new SpriteState { Texture = goombaSprites, SourceRect = goombaAnimationConfig.States["walking"].Frames[0], Color = Color.White });
        Goomba.AddComponent(new AnimationConfig { States = goombaAnimationConfig.States});
        Goomba.AddComponent(new AnimationState { CurrentState = "walking"});

        //Load in Koopa
        var koopaSprites = Content.Load<Texture2D>("Sprites/koopas");
        var koopaAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/koopa_spritesheet.json"));

        Koopa.AddComponent(new BasicEnemy { Name = "Koopa", Value = enemyVelocity });
        Koopa.AddComponent(new Rotation { Value = 0.0f });
        Koopa.AddComponent(new Scale { Value = new Vector2(3, 3) });
        Koopa.AddComponent(new SpriteState { Texture = koopaSprites, SourceRect = koopaAnimationConfig.States["walking"].Frames[0], Color = Color.White });
        Koopa.AddComponent(new AnimationConfig { States = koopaAnimationConfig.States});
        Koopa.AddComponent(new AnimationState { CurrentState = "walking"});


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
