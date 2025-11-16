namespace ECS.Systems;

public class RiseSystem : AbstractSystem
{
    private Rectangle _cameraBounds;

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<CameraBoundsEvent>(OnCameraBounds);
        return true;
    }
    private void OnCameraBounds(IEvent evnt)
    {
        var e = (CameraBoundsEvent)evnt;
        _cameraBounds = e.WorldBounds;
    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var e in engine.GetEntities())
        {
            if (!e.TryGetComponent<Rise>(out var rise)) continue;
            if (!e.TryGetComponent<Position>(out var pos))
            { e.RemoveComponent<Rise>(); continue; }

            var posPoint = new Point((int)pos.Value.X, (int)pos.Value.Y);
            if (!_cameraBounds.Contains(posPoint))
                continue;
            float dy = rise.Speed * dt;
            float remaining = rise.DeltaY - rise.Moved;
            float move = MathF.Min(dy, remaining);

            pos.Value.Y -= move;
            rise.Moved += move;
            if (rise.Moved >= rise.DeltaY)
            {
                e.RemoveComponent<Rise>();
                Engine.PubSub.Publish(new PostRiseEvent { Entity = e });
            }
        }
    }
}
