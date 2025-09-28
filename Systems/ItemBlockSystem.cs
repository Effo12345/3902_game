namespace ECS.Systems;

public class ItemBlockSystem : AbstractSystem
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
        if (!e.TryGetComponent<ItemBlock>(out var ib))
            return;
        if (!e.TryGetComponent<SpriteState>(out var sprite))
            return;
        if (!e.TryGetComponent<Position>(out var pos))
            return;
        if (ib.Used)
            return;
        ib.Used = true;
        sprite.SourceRect = ib.UsedBlockRect;
        e.RemoveComponent<Bump>();

        var topPos = pos.Value - new Vector2(0f, ib.ItemRect.Height * 2);
        Engine.PubSub.Publish(new ItemSpawn
        {
            Position = new Position { Value = topPos },
            Texture = ib.ItemTexture,
            SourceRect = ib.ItemRect,
            Kind = ib.Kind
        });
    }
    public override void Update(Engine engine, GameTime gameTime) {}
}