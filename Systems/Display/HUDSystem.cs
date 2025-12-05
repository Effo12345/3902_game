namespace ECS.Systems;

public class HUDSystem : AbstractSystem
{
    private TextRender _scoreText;
    private TextRender _coinsText;
    private TextRender _worldText;
    private TextRender _timeText;
    private Entity _scoreEntity;
    private Entity _timerEntity;
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
        _coinsText = null; _worldText = null; _timeText = null; _scoreEntity = null; _timerEntity = null;
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
                _scoreEntity = ent;

            if (ent.HasComponent<LevelTimer>())
                _timerEntity = ent;

            if (_scoreEntity != null && _timerEntity != null)
                break;
        }

        RefreshHudText();
    }
    private void RefreshHudText()
    {
        int scoreValue = 0;
        int coinsValue = 0;
        int displayTime = 0;

        if (_scoreEntity != null && _scoreEntity.TryGetComponent<Score>(out var score) &&
            _scoreEntity.TryGetComponent<Coin>(out var coin))
        {
            scoreValue = score.Value;
            coinsValue = coin.Value;
        }

        _scoreText.Text = $"{scoreValue:D6}";
        _coinsText.Text = $"x{coinsValue:D2}";
        
        if (_timerEntity != null &&
            _timerEntity.TryGetComponent<LevelTimer>(out var timer))
        {
            displayTime = (int)MathF.Ceiling(timer.RemainingTime);
        }
        _timeText.Text = $"{displayTime}";
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        RefreshHudText();
    }
}