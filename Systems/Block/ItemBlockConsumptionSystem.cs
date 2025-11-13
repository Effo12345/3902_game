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
    public override void Update(Engine engine, GameTime gameTime){}
}