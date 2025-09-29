namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Engine Engine = new();
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
