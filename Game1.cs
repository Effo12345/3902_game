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

        Engine.AddSystem(new DemoInputSystem());
        Engine.AddSystem(new DemoMovementSystem());
        Engine.AddSystem(new EnemyMovementSystem());

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        Engine.AddSystem(new RenderSystem(_spriteBatch));

        var marioSprites = Content.Load<Texture2D>("Sprites/mario");
        var marioAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/mario_spritesheet.json"));

        TestEntity.AddComponent(new Position { Value = new Vector2(20, 20) });
        TestEntity.AddComponent(new Velocity { Value = Vector2.Zero });
        TestEntity.AddComponent(new Rotation { Value = 0.0f });
        TestEntity.AddComponent(new Scale { Value = new Vector2(3, 3) });
        TestEntity.AddComponent(new SpriteState { Texture = marioSprites, SourceRect = marioAnimationConfig.States["idle"].Frames[0], Color = Color.White });

        //Load in Goomba
        var goombaSprites = Content.Load<Texture2D>("Sprites/goombas");
        var goombaAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/goomba_spritesheet.json"));

        Goomba.AddComponent(new Position { Value = new Vector2(100, 100) });
        Goomba.AddComponent(new BasicEnemyMovement { Value = enemyVelocity });
        Goomba.AddComponent(new Rotation { Value = 0.0f });
        Goomba.AddComponent(new Scale { Value = new Vector2(1, 1) });
        Goomba.AddComponent(new SpriteState { Texture = goombaSprites, SourceRect = goombaAnimationConfig.States["walking"].Frames[0], Color = Color.White });

        //Load in Koopa
        var koopaSprites = Content.Load<Texture2D>("Sprites/koopas");
        var koopaAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/koopa_spritesheet.json"));

        Koopa.AddComponent(new Position { Value = new Vector2(200, 200) });
        Koopa.AddComponent(new BasicEnemyMovement { Value = enemyVelocity});
        Koopa.AddComponent(new Rotation { Value = 0.0f });
        Koopa.AddComponent(new Scale { Value = new Vector2(1, 1) });
        Koopa.AddComponent(new SpriteState { Texture = koopaSprites, SourceRect = koopaAnimationConfig.States["walking"].Frames[0], Color = Color.White });


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
