namespace ECS.Systems;

public class ScoreSystem : AbstractSystem
{
    private Entity _scoreEntity;
    private int _score;
    private int _coins;

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);

        engine.PubSub.CreateSubscription<LevelLoadedEvent>(OnLevelLoaded);
        engine.PubSub.CreateSubscription<CoinPickupEvent>(OnCoinCollected);
        engine.PubSub.CreateSubscription<ScoreAddedEvent>(OnScoreAdded);

        return true;
    }
    private void OnLevelLoaded(IEvent evt)
    {
        _scoreEntity = Engine.CreateEntity();
        _scoreEntity.AddComponent(new Score { Value = _score });
        _scoreEntity.AddComponent(new Coin { Value = _coins });
    }
    private void OnCoinCollected(IEvent evt)
    {
        var e = (CoinPickupEvent)evt;
        var score = _scoreEntity.GetComponent<Score>();
        var coins = _scoreEntity.GetComponent<Coin>();

        score.Value += e.CoinValue;
        coins.Value += e.CoinCount;
    }

    private void OnScoreAdded(IEvent evt)
    {
        var e = (ScoreAddedEvent)evt;
        var score = _scoreEntity.GetComponent<Score>();
        score.Value += e.ScoreValue;
    }

    public override void Update(Engine engine, GameTime gameTime) { }
}