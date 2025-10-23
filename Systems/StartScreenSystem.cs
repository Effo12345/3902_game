namespace ECS.Systems;

public sealed class StartScreenSystem : AbstractSystem
{
    private readonly Game _game;
    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphics;
    private int _cursorOffset = 30, _selected = 0;
    private KeyboardState _prev;
    private bool _done = false;

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);

        StartScreenRender.BuildScreen(engine, _content, _graphics);
        StartScreenRender.MoveCursor(_selected, _cursorOffset);

        return true;
    }
    public StartScreenSystem(Game game, ContentManager content, GraphicsDevice graphics)
    {
        _game = game;
        _content = content;
        _graphics = graphics;
    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        if (_done) return;
        var count = StartScreenRender.OptionsCount();
        var kb = Keyboard.GetState();

        if (kb.IsKeyDown(Keys.Up) && !_prev.IsKeyDown(Keys.Up))
        {
            _selected = Math.Max(0, _selected - 1);
            StartScreenRender.MoveCursor(_selected, _cursorOffset);
        }
        if (kb.IsKeyDown(Keys.Down) && !_prev.IsKeyDown(Keys.Down))
        {
            _selected = Math.Min(count - 1, _selected + 1);
            StartScreenRender.MoveCursor(_selected, _cursorOffset);
        }
        if(kb.IsKeyDown(Keys.Space) && !_prev.IsKeyDown(Keys.Space))
        {
            if (_selected == 0)
            {
                StartScreenRender.DestroyAll(engine);
                LevelLoader.Load(engine, _content, "Content/Config/Level/TestLevel.json");
            }
            else
            {
                _game.Exit();
            }
            _done = true;
        }
        _prev = kb;
    }
}