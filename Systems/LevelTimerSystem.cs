public class LevelTimerSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<EndLevelTimer>(EndTimer);
        return true;
    }
    private void EndTimer(IEvent _)
    {

        foreach (var ent in Engine.GetEntities())
        {
            if (ent.TryGetComponent<LevelTimer>(out var timer))
            {
                timer.IsCounting = false; 
            }
        }
    }
    private void DeathOnZero()
    {
        foreach (var entity in Engine.GetEntities())
        {
            if (entity.TryGetComponent<Tag>(out var tag) && tag.Value == CONSTANTS.TAG.PLAYER
            && entity.TryGetComponent<Status>(out var status))
            {
                status.State = CONSTANTS.PLAYER_STATE.DEAD;
                break;
            }
        }
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var ent in engine.GetEntities())
        {
            if (!ent.TryGetComponent<LevelTimer>(out var timer) || !timer.IsCounting)
                continue;

            timer.RemainingTime -= dt;

            if (timer.RemainingTime < 0)
            {
                timer.RemainingTime = 0;
                DeathOnZero();
            }
        }
    }

}