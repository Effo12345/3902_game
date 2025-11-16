
namespace ECS.Systems;
public class ItemPostRiseSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<PostRiseEvent>(OnRiseCompleted);
        return true;
    }

    private void OnRiseCompleted(IEvent evnt)
    {
        var e = ((PostRiseEvent)evnt).Entity;

        if (!e.HasComponent<ItemKind>() || !e.HasComponent<CanMove>() || e.HasComponent<Velocity>()) return;

        e.AddComponent(new Velocity { Value = CONSTANTS.COLLISION.ITEM_VELOCITY });
    }
    public override void Update(Engine engine, GameTime gameTime){}
}
