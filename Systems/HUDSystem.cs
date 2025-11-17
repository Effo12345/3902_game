namespace ECS.Systems;

public class HUDSystem : AbstractSystem
{
    private TextRender _scoreText;
    private TextRender _coinsText;
    private TextRender _worldText;
    private TextRender _timeText;
    private int _score, _coins, _timeLimit = 400;
    private float _timeRemaining;
    private string _world;
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<LevelLoadedEvent>(OnLevelLoaded);
        return true;
    }
    private void OnLevelLoaded(IEvent evnt)
    {
        var e = (LevelLoadedEvent)evnt;

        _score = 0;
        _coins = 0;
        _world = e.Level;
        _timeRemaining = _timeLimit;

        foreach (var ent in Engine.GetEntities())
        {
            if (!ent.TryGetComponent<TextRender>(out var text)) continue;
            if (!ent.TryGetComponent<Tag>(out var tag)) continue;

            switch (tag.Value)
            {
                case "hud_score_value":
                    _scoreText = text;
                    break;
                case "hud_coins_value":
                    _coinsText = text;
                    break;
                case "hud_world_value":
                    _worldText = text;
                    break;
                case "hud_time_value":
                    _timeText = text;
                    break;
            }
        }
        RefreshHudText();
    }
    private void RefreshHudText()
    {
        _scoreText.Text = $"{_score:D6}";
        _coinsText.Text = $"x{_coins:D2}";
        int displayTime = (int)MathF.Ceiling(_timeRemaining);

        if (displayTime > 0)
            _timeText.Text = $"{displayTime:D3}";
    }
    public void AddScore(int amount) =>  _score += amount;
    public void AddCoin(int amount) => _coins += amount;
    public override void Update(Engine engine, GameTime gameTime)
    {
        _timeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_timeRemaining < 0)
            _timeRemaining = 0;

        RefreshHudText();
    }
}