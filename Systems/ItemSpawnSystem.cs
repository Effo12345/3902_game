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
        if (e.SourceBlock.TryGetComponent<Collider>(out var blockCollider) && item.TryGetComponent<Collider>(out var itemCollider))
        {
            item.AddComponent(new Rise { DeltaY = itemCollider.HitBox.Height, Speed = 30f });
        }
        Vector2 spawnPosition = new Vector2(blockCollider.HitBox.X, blockCollider.HitBox.Y);

        if (item.TryGetComponent<Position>(out var position))
            position.Value = spawnPosition;
        else
            item.AddComponent(new Position { Value = spawnPosition });
    
    }
    public override void Update(Engine engine, GameTime gameTime) { }
} 