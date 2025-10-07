namespace ECS.Util;

public static class PlayerFactory
{
    // TODO: Increase modularity (builder pattern?) to set things like position
    public static Entity MakePlayer(Engine engine, Texture2D texture, string animationConfigFile, string controlConfigFile)
    {
        Entity entity = engine.CreateEntity();

        var marioAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText(animationConfigFile));
        var marioControlConfig = InputDefinitionLoader.Load(File.ReadAllText(controlConfigFile));

        entity.AddComponent(new Position { Value = new Vector2(20, 20) });
        entity.AddComponent(new Velocity { Value = Vector2.Zero });
        entity.AddComponent(new Rotation { Value = 0.0f });
        entity.AddComponent(new Scale { Value = new Vector2(3, 3) });
        entity.AddComponent(new SpriteState { Texture = texture, Color = Color.White, Origin = new Vector2(0, 0) });
        entity.AddComponent(new Mass { Value = 5 });
        entity.AddComponent(new Acceleration());
        entity.AddComponent(new IsGrounded());
        entity.AddComponent(marioControlConfig);
        entity.AddComponent(new Friction { Value = 4.0f });
        entity.AddComponent(new VelocityCap { Value = new Vector2(0.2f, 10.0f) });
        entity.AddComponent(marioAnimationConfig);
        entity.AddComponent(new AnimationState { CurrentState = "small_idle" });
        entity.AddComponent(new FacingDirection());
        entity.AddComponent(new Status { State = "small" });

        return entity;
    }
}