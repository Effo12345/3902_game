namespace ECS.Systems;

public class HUDSystem : AbstractSystem
{
    private TextRender _scoreText;
    private TextRender _coinsText;
    private TextRender _worldText;
    private TextRender _timeText;
    private int _timeLimit = 400;
    private float _timeRemaining;
    private Entity _scoreEntity;
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
        foreach (var ent in Engine.GetEntities())
        {
            if (ent.HasComponent<Score>() && ent.HasComponent<Coin>())
            {
                _scoreEntity = ent;
                break;
            }
        }
        RefreshHudText();
    }
    private void RefreshHudText()
    {
        if (_scoreText == null || _coinsText == null || _timeText == null)
            return;

        int scoreValue = 0;
        int coinsValue = 0;

        if (_scoreEntity != null && _scoreEntity.TryGetComponent<Score>(out var score) &&
            _scoreEntity.TryGetComponent<Coin>(out var coin))
        {
            scoreValue = score.Value;
            coinsValue = coin.Value;
        }

        _scoreText.Text = $"{scoreValue:D6}";
        _coinsText.Text = $"x{coinsValue:D2}";
        
        int displayTime = (int)MathF.Ceiling(_timeRemaining);
        if (displayTime > 0)
            _timeText.Text = $"{displayTime:D3}";
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        _timeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (_timeRemaining < 0)
            _timeRemaining = 0;

        RefreshHudText();
    }
}