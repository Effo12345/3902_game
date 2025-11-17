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
            if (item.TryGetComponent<Mass>(out var mass) &&
                itemCollider.Type == ColliderType.Coin)
            {
                item.AddComponent(new Velocity {Value = new Vector2(0, CONSTANTS.COLLISION.ITEM_SPAWN_VELOCITY)});
                // mass.Value = CONSTANTS.COLLISION.ITEM_MASS;

                SoundPlayer.PlaySfx(CONSTANTS.SOUND.COIN);
            }
            else
            {
                var speed = CONSTANTS.COLLISION.ITEM_RISE_SPEED;
                if (itemCollider.Type == ColliderType.Coin)
                {
                    speed = CONSTANTS.COLLISION.COIN_RISE_SPEED;
                }
                item.AddComponent(new Rise { DeltaY = blockCollider.HitBox.Height, Speed = speed });

                SoundPlayer.PlaySfx(CONSTANTS.SOUND.POWERUP_SPAWN);
            }
        }
        Vector2 spawnPosition = new Vector2(blockCollider.HitBox.X, blockCollider.HitBox.Y);

        if (item.TryGetComponent<Position>(out var position))
            position.Value = spawnPosition;
        else
            item.AddComponent(new Position { Value = spawnPosition });
    
    }
    public override void Update(Engine engine, GameTime gameTime)
    {

    }
} 