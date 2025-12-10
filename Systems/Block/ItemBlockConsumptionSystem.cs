namespace ECS.Systems;

public class ItemBlockConsumptionSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<BlockBumped>(OnBlockBumped);
        return true;
    }

    private void OnBlockBumped(IEvent evnt)
    {
        var e = ((BlockBumped)evnt).Entity;
        var pos = ((BlockBumped)evnt).Position;

        if (!e.TryGetComponent<HitCount>(out var hits) || !e.TryGetComponent<ItemKind>(out var kind) ||
            hits.Value <= 0)
            return;

        hits.Value--;

        if (Math.Abs(hits.Timer) < 0.001)
            hits.Value = 0;
        
        hits.Timer = CONSTANTS.TEMP.BLOCK_TIMER; //For the ten-coin blocks: This timer gets reset every time mario hits the block (1 sec). If it reaches 0, the next time mario hits the block it will turn off.
        Engine.PubSub.Publish(new ItemSpawn
        {
            Position = pos,
            ItemKind = kind,
            SourceBlock = e,
            TextureKey = kind.TextureKey,
            SpriteAtlasKey = kind.SpriteAtlasKey
        });

        if (hits.Value == 0)
        {
            if (e.HasComponent<ItemKind>()) e.RemoveComponent<ItemKind>();
            if (e.HasComponent<Bump>()) e.RemoveComponent<Bump>();
            if (e.TryGetComponent<AnimationState>(out var anim))
            {
                anim.CurrentState = CONSTANTS.ANIMATION_STATE.USED_BLOCK;
            }
        }
    }

    // no-op
    public override void Update(Engine engine, GameTime gameTime){}
}