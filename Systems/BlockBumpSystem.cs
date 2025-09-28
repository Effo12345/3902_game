namespace ECS.Systems;

public class BlockBumpSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<BlockBump>(OnBump);
        return true;
    }
    private void OnBump(IEvent evnt)
    {
        var e = ((BlockBump)evnt).Entity;

        if (!e.TryGetComponent<Bump>(out var config))
            return;
        if (!e.TryGetComponent<Position>(out var pos))
            return;
        if (e.TryGetComponent<BumpState>(out _))
            return;
        e.AddComponent(new BumpState
        {
            InitY = pos.Value.Y,
            PeakY = pos.Value.Y - MathF.Abs(config.Height),
            CurrentY = pos.Value.Y,
            Speed = -MathF.Abs(config.UpSpeed)
        });
    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var e in engine.GetEntities())
        {
            if (!e.TryGetComponent<BumpState>(out var state))
                continue;
            if (!e.TryGetComponent<Position>(out var pos))
            {
                e.RemoveComponent<BumpState>();
                continue;
            }
            if (!e.TryGetComponent<Bump>(out var config))
            {
                e.RemoveComponent<BumpState>();
                continue;
            }
            state.CurrentY += state.Speed * dt;
            if (state.Speed < 0 && state.CurrentY <= state.PeakY)
            {
                state.CurrentY = state.PeakY;
                state.Speed = MathF.Abs(config.DownSpeed);
            }
            if (state.CurrentY > 0 && state.CurrentY >= state.InitY)
            {
                state.CurrentY = state.InitY;
                pos.Value.Y = state.CurrentY;
                e.RemoveComponent<BumpState>();
                continue;
            }
            pos.Value.Y = state.CurrentY;
        }
    }
}