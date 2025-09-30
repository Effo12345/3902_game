namespace ECS.Systems;

public class ItemBlockSystem : AbstractSystem
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
        //Check if it is an item block
        if (!e.TryGetComponent<ItemBlock>(out var ib))
            return;
        if (!e.TryGetComponent<SpriteState>(out var sprite))
            return;
        if (!e.TryGetComponent<Position>(out var pos))
            return;
        if (ib.Used)
            return;
        //Mark block is used 
        ib.Used = true;
        //Update block sprite to used block
        sprite.SourceRect = ib.UsedBlockRect;
        //Remove Bump so block no longer reacts to bump
        e.RemoveComponent<Bump>();
        //Compute spawn position
        var topPos = pos.Value - new Vector2(0f, ib.ItemRect.Height * 2);
        //Publish ItemSpawn event
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