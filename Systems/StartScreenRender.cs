namespace ECS.Systems;
public static class StartScreenRender
{
    private static Entity _title;
    private static Entity _cursor;
    private static readonly List<Entity> _options = new();

    public static void BuildScreen(Engine engine, ContentManager content, GraphicsDevice graphics)
    {
        var vp = graphics.Viewport;

        // Title
        var titleTex = content.Load<Texture2D>("Sprites/title");
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

        // Font for labels
        var font = content.Load<SpriteFont>("Fonts/TextFont");

        AddTextOption(engine, font, "START GAME", new Vector2(vp.Width * 0.5f, vp.Height * 0.65f));
        AddTextOption(engine, font, "QUIT GAME",  new Vector2(vp.Width * 0.5f, vp.Height * 0.75f));

        // Cursor
        var tilesTex  = content.Load<Texture2D>("Sprites/tiles");
        var itemsAnim = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/tiles_spritesheet.json"));
        var mushroom  = itemsAnim.States["SuperMushroom"].Frames[0];

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
    public static void MoveCursor(int selected, int cursorOffset)
    {
        if (_cursor == null || _options.Count == 0) return;
        if (selected < 0 || selected >= _options.Count) return;

        var sel = _options[selected];
        if (!_cursor.TryGetComponent<Position>(out var cpos)) return;
        if (!sel.TryGetComponent<TextLabel>(out var tl)) return;
        if (!sel.TryGetComponent<Position>(out var pos)) return;


        var size = tl.Font.MeasureString(tl.Text);
        float x = pos.Value.X - (size.X * 0.5f) - cursorOffset;
        float y = pos.Value.Y;
        cpos.Value = new Vector2(x, y);
    }
    public static int OptionsCount()
    {
        return _options.Count;
    } 
    private static void AddTextOption(Engine engine, SpriteFont font, string text, Vector2 pos)
    {
        var e = engine.CreateEntity();
        e.AddComponent(new Position { Value = pos });
        var tl = new TextLabel { Text = text, Font = font, Color = Color.Yellow };
        tl.Origin = font.MeasureString(tl.Text) * 0.5f;
        e.AddComponent(tl);
        _options.Add(e);
    }
    public static void DestroyAll(Engine engine)
    {
        engine.DestroyEntity(_title);
        engine.DestroyEntity(_cursor);
        foreach (var e in _options) engine.DestroyEntity(e);
    }
}
