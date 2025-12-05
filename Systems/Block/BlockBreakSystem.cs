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

        if(!entity.TryGetComponent<Position>(out var pos) || !entity.TryGetComponent<BlockDebris>(out var debris))
            return;
        Engine.DestroyEntity(entity);
        SoundPlayer.PlaySfx(CONSTANTS.SOUND.BLOCKBREAK);
        //TODO: add debris on block break
        SpawnDebris(pos.Value, debris);
    }
    private void SpawnDebris(Vector2 position, BlockDebris blockDebris)
    {
        int count = blockDebris.Directions.Length;
        for(int i = 0; i < count; i++)
        {
            var debris = EntityFactory.BuildEntity(Engine, CONSTANTS.TAG.BLOCK_DEBRIS);

            if(!debris.TryGetComponent<Position>(out var pos) || !debris.TryGetComponent<Velocity>(out var vel)
            || !debris.TryGetComponent<Acceleration>(out var accel))
            {
                Engine.DestroyEntity(debris);
                return;
            }
            pos.Value = position;
            vel.Value = blockDebris.Directions[i] * blockDebris.FragSpeed;
            accel.Value = new Vector2(0f, blockDebris.Acceleration);
        }
    }
    public override void Update(Engine engine, GameTime gameTime) { }
}