namespace ECS.Systems;

public class ItemSpawnSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        //Call OnSpawn when ItemSpawn is triggered
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<ItemSpawn>(OnSpawn);
        return true;
    }
    private void OnSpawn(IEvent evnt)
    {
        //events carries the info to create the item
        var e = (ItemSpawn)evnt;
        //create item using provided info
        CreateItem(e.Position, e.Texture, e.SourceRect);
    }
    private void CreateItem(Position p, Texture2D t, Rectangle r)
    {
        //Create new entity for item
        var e = Engine.CreateEntity();
        //Give entity necessary components
        // TODO: Find a better place to keep this component data
        e.AddComponent(p);
        e.AddComponent(new Rotation { Value = 0f });
        e.AddComponent(new Scale { Value = new Vector2(2.5f, 2.5f) });
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