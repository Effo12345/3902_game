// CURRENTLY ARCHIVED
// MOSTLY FUNCTIONAL BUT NEEDS TWEAKS FOR THE NEW SYSTEMS

namespace ECS.Systems;

//System responsible for handling block breaking
public class BlockBreakSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        //Call OnBreak when BlockBreak is triggered
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<BlockBreak>(OnBreak);
        return true;
    }
    private void OnBreak(IEvent evnt)
    {
        var e = ((BlockBreak)evnt).Entity;

        if (!e.TryGetComponent<Break>(out var br) || !e.TryGetComponent<SpriteState>(out var sprite)
        || !e.TryGetComponent<Position>(out var pos))
            return;

        Engine.PubSub.Publish(new DebrisSpawn { Position = pos, Sprite = sprite, Break = br });
        //remove original sprite
        e.RemoveComponent<SpriteState>();
    }
    public override void Update(Engine engine, GameTime time) { }
}