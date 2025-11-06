namespace ECS.Systems;

public class ItemSpawnSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<ItemSpawn>(OnSpawn);
        return true;
    }
    private void OnSpawn(IEvent evnt)
    {
        var e = (ItemSpawn)evnt;

        Entity item = EntityFactory.BuildEntity(Engine, e.ItemKind.Kind.ToString());
        Vector2 pos = e.Position.Value;
        if (e.SourceBlock.TryGetComponent<Collider>(out var blockCol) && item.TryGetComponent<Collider>(out var itemCol))
        {
            float blockWidth = blockCol.HitBox.Width;
            float itemWidth = itemCol.HitBox.Width;
            float itemHeight = itemCol.HitBox.Height;

            pos.X += (blockWidth - itemWidth) / 2;
            item.AddComponent(new Rise { DeltaY = itemHeight, Speed = 30f });
        }

        if (item.TryGetComponent<Position>(out var p))
            p.Value = pos;
        else
            item.AddComponent(new Position { Value = pos });
    
    }
    public override void Update(Engine engine, GameTime gameTime) { }
} 