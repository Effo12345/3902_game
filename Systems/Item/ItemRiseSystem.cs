namespace ECS.Systems;

public class ItemRiseSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var e in engine.GetEntities())
        {
            if (!e.TryGetComponent<Rise>(out var rise)) continue;
            if (!e.TryGetComponent<Position>(out var pos))
            { e.RemoveComponent<Rise>(); continue; }

            float dy = rise.Speed * dt;
            float remaining = rise.DeltaY - rise.Moved;
            float move = MathF.Min(dy, remaining);

            pos.Value.Y -= move;
            rise.Moved += move;
            if (rise.Moved >= rise.DeltaY)
            {
                e.RemoveComponent<Rise>();
                if (e.HasComponent<CanMove>())
                    e.AddComponent(new Velocity { Value = CONSTANTS.COLLISION.ITEM_VELOCITY });
            }
        }
    }
}
