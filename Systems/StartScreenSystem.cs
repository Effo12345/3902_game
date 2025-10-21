using System.Threading;

namespace ECS.Systems;

public sealed class StartScreenSystem : AbstractSystem
{
    private readonly Game _game;
    private readonly ContentManager _content;
    private readonly GraphicsDevice _graphics;
    private SpriteFont font;
    private Entity _title;
    private List<Entity> _selectionList = new();
    private Entity _cursor;
    private int _cursorOffset = 30;
    private int _selected = 0;
    private KeyboardState _prev;
    private bool _done = false;

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        BuildScreen(engine);
        MoveCursorToSelected();
        return true;
    }
    public StartScreenSystem(Game game, ContentManager content, GraphicsDevice graphics)
    {
        _game = game;
        _content = content;
        _graphics = graphics;
    }
    private void BuildScreen(Engine engine)
    {
        var titleTex = _content.Load<Texture2D>("Sprites/title");
        var vp = _graphics.Viewport;

        _title = engine.CreateEntity();
        _title.AddComponent(new SpriteState
        {
            Texture = titleTex,
            SourceRect = new Rectangle(0, 0, titleTex.Width, titleTex.Height),
            Color = Color.White,
            Origin = new Vector2(titleTex.Width * 0.5f, titleTex.Height * 0.5f)
        });
        _title.AddComponent(new Position { Value = new Vector2(vp.Width * 0.5f, vp.Height * 0.3f) });
        _title.AddComponent(new Scale { Value = new Vector2(0.85f, 0.85f) });
        _title.AddComponent(new Rotation { Value = 0f });

        font = _content.Load<SpriteFont>("Fonts/TextFont");

        //add start game text label
        var startGameText = engine.CreateEntity();
        startGameText.AddComponent(new Position { Value = new Vector2(vp.Width * 0.5f, vp.Height * 0.65f) });
        TextLabel tl1 = new TextLabel { Text = "START GAME", Font = font, Color = Color.Yellow };
        tl1.Origin = font.MeasureString(tl1.Text) * 0.5f;
        startGameText.AddComponent(tl1);
        _selectionList.Add(startGameText);

        //add quit game text label
        var quitGameText = engine.CreateEntity();
        quitGameText.AddComponent(new Position { Value = new Vector2(vp.Width * 0.5f, vp.Height * 0.75f) });
        TextLabel tl2 = new TextLabel { Text = "QUIT GAME", Font = font, Color = Color.Yellow };
        tl2.Origin = font.MeasureString(tl2.Text) * 0.5f;
        quitGameText.AddComponent(tl2);
        _selectionList.Add(quitGameText);

        //add cursor
        var tilesTex = _content.Load<Texture2D>("Sprites/tiles");
        var itemsAnim = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/tiles_spritesheet.json"));
        var mushroom = itemsAnim.States["SuperMushroom"].Frames[0];

        _cursor = engine.CreateEntity();
        _cursor.AddComponent(new SpriteState
        {
            Texture = tilesTex,
            SourceRect = mushroom,
            Color = Color.White,
            Origin = new Vector2(mushroom.Width * 0.5f, mushroom.Height * 0.5f)
        });
        _cursor.AddComponent(new Scale { Value = new Vector2(2f, 2f) });
        _cursor.AddComponent(new Rotation { Value = 0f });
        _cursor.AddComponent(new Position { Value = Vector2.Zero });
    }
    private void MoveCursorToSelected()
    {
        var selEntity = _selectionList[_selected];
        if (!_cursor.TryGetComponent<Position>(out var cpos))
            return;
        if (!selEntity.TryGetComponent<TextLabel>(out var tl))
            return;
        if (!selEntity.TryGetComponent<Position>(out var pos))
            return;
        var stringLength = font.MeasureString(tl.Text);
        float x = pos.Value.X - (stringLength.X * 0.5f) - _cursorOffset;
        float y = pos.Value.Y;
        cpos.Value = new Vector2(x, y);
    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        if (_done)
        {
            return;
        }

        var kb = Keyboard.GetState();

        if (kb.IsKeyDown(Keys.Up) && !_prev.IsKeyDown(Keys.Up))
        {
            _selected -= 1;
            if (_selected < 0)
                _selected = 0;
            MoveCursorToSelected();
        }
        if (kb.IsKeyDown(Keys.Down) && !_prev.IsKeyDown(Keys.Down))
        {
            _selected += 1;
            if (_selected >= _selectionList.Count)
                _selected = _selectionList.Count - 1;
            MoveCursorToSelected();
        }
        if(kb.IsKeyDown(Keys.Space) && !_prev.IsKeyDown(Keys.Space))
        {
            ActivateSelected(engine);
        }
        _prev = kb;
    }
    private void ActivateSelected(Engine engine)
    {
        if (_selectionList.Count == 0)
            return;
        if (_selected == 0)
        {
            engine.DestroyEntity(_title);
            engine.DestroyEntity(_cursor);
            foreach (var e in _selectionList)
            {
                engine.DestroyEntity(e);
            }
            LevelLoader.Load(engine, _content, "Content/Config/Level/TestLevel.json");
        }
        else
        {
            _game.Exit();
        }
        _done = true;
    }
}