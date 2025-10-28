using ECS.Components;

namespace ECS.Systems;

public class FireballSystem : AbstractSystem
{
    private Texture2D fireballsprite;

    public FireballSystem(Texture2D fireballsprite)
    {
        this.fireballsprite = fireballsprite;
    }

    public override bool Initialize(Engine engine)
    {
        //Call OnSpawn when ItemSpawn is triggered
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<FireballLaunch>(OnSpawn);
        return true;
    }
    private void OnSpawn(IEvent evnt)
    {
        //events carries the info to create the item
        var e = (FireballLaunch)evnt;
        //create item using provided info
        var fireball = Engine.CreateEntity();
        var itemConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/items_spritesheet.json"));

        // TODO: Fix the spawn position for fireball
        fireball.AddComponent(new Position { Value = e.Position.Value });
        fireball.AddComponent(new Rotation { Value = 0f });
        fireball.AddComponent(new Scale { Value = new Vector2(1.5f, 1.5f) });
        fireball.AddComponent(new Velocity { Value = e.Direction * 250f });
        fireball.AddComponent(new VelocityCap());
        fireball.AddComponent(new Acceleration());
        fireball.AddComponent(new Mass { Value = 2.5f });
        fireball.AddComponent(new SpriteState { Texture = fireballsprite, SourceRect = itemConfig.States["Fireball"].Frames[0], Color = Color.White });
        fireball.AddComponent(new AnimationConfig { States = itemConfig.States });
        fireball.AddComponent(new AnimationState { CurrentState = "Fireball" });
    }
    public override void Update(Engine engine, GameTime gameTime){}
}