namespace ECS.Systems;

public class BlockBumpMotionSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        //Call OnBump when BlockBump is triggered
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<BlockBump>(OnBump);
        return true;
    }
    private void OnBump(IEvent evnt)
    {
        //Get bumped entity
        var e = ((BlockBump)evnt).Entity;

        if (!e.TryGetComponent<Bump>(out var config) || !e.TryGetComponent<Position>(out var pos)
        || e.TryGetComponent<BumpState>(out _))
            return;

        e.AddComponent(new BumpState
        {
            InitY = pos.Value.Y,
            PeakY = pos.Value.Y - MathF.Abs(config.Height),
            CurrentY = pos.Value.Y,
            Speed = -MathF.Abs(config.UpSpeed),
            UpSpeed = MathF.Abs(config.UpSpeed),
            DownSpeed = MathF.Abs(config.DownSpeed)
        });
        Engine.PubSub.Publish(new BlockBumped { Entity = e, Position = pos });
    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        //loop all entities
        foreach (var e in engine.GetEntities())
        {
            //check if entity has a BumpState
            if (!e.TryGetComponent<BumpState>(out var state))
                continue;
            if (!e.TryGetComponent<Position>(out var pos))
            {
                e.RemoveComponent<BumpState>();
                continue;
            }
            //Update current Y using speed and delta time
            state.CurrentY += state.Speed * dt;
            //If moving upward and Y have not reach peak
            if (state.Speed < 0 && state.CurrentY <= state.PeakY)
            {
                //Switch to downward speed once peak reached
                state.CurrentY = state.PeakY;
                state.Speed = state.DownSpeed;
            }
            //If moving downward and have not reach original pos
            if (state.Speed > 0 && state.CurrentY >= state.InitY)
            {
                //Remove BumpState when reaching original pos
                state.CurrentY = state.InitY;
                pos.Value.Y = state.CurrentY;
                e.RemoveComponent<BumpState>();
                continue;
            }
            //Update Y position
            pos.Value.Y = state.CurrentY;
        }
    }
}