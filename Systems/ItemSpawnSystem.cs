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
        CreateItem(e.Position, e.Texture, e.SourceRect);
    }
    private void CreateItem(Position p, Texture2D t, Rectangle r)
    {
        var e = Engine.CreateEntity();

        e.AddComponent(p);
        e.AddComponent(new Rotation { Value = 0f });
        e.AddComponent(new Scale { Value = new Vector2(2, 2) });
        e.AddComponent(new SpriteState
        {
            Texture = t,
            SourceRect = r,
            Origin = Vector2.Zero,
            Color = Color.White
        });
    }
    public override void Update(Engine engine, GameTime gameTime){}
}