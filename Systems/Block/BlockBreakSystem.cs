namespace ECS.Systems;

public class BlockBreakSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        //Call OnBump when BlockBump is triggered
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<BlockBreak>(HandleBlockBreak);
        return true;
    }
    private void HandleBlockBreak(IEvent evnt)
    {
        //Get broken entity
        var entity = ((BlockBreak)evnt).Entity;

        Engine.DestroyEntity(entity);
        SoundPlayer.PlaySfx(CONSTANTS.SOUND.BLOCKBREAK);
        //TODO: add debris on block break

    }
    public override void Update(Engine engine, GameTime gameTime) { } //NO OP
}