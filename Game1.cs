namespace ECS;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Engine Engine = new();
    private Entity TestEntity;

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
        TestEntity.AddComponent(new Rotation { Value = 0.0f });
        TestEntity.AddComponent(new Scale { Value = new Vector2(1, 1) });
        TestEntity.AddComponent(new SpriteState { Texture = marioSprites, SourceRect = marioAnimationConfig.States["idle"].Frames[0], Color = Color.White });
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
