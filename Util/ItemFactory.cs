namespace ECS.Util;

public static class ItemFactory
{
    public static Entity MakeItem(Engine engine, Vector2 pos, Texture2D itemSprite, string itemConfigFile, string itemInputFile)
    {
        Entity entity = engine.CreateEntity();

        var itemConfig = SpriteSheetLoader.Load(File.ReadAllText(itemConfigFile));

        var itemInput = InputDefinitionLoader.Load(File.ReadAllText(itemInputFile));

        // TODO: pull out the remaining magic numbers
        entity.AddComponent(new Position { Value = pos });
        entity.AddComponent(new Velocity { Value = Vector2.Zero });
        entity.AddComponent(new Rotation { Value = 0.0f });
        entity.AddComponent(new Scale { Value = new Vector2(2.5f, 2.5f) });
        entity.AddComponent(new SpriteState { Texture = itemSprite, Color = Color.White });
        entity.AddComponent(new AnimationConfig { States = itemConfig.States });
        entity.AddComponent(new AnimationState { CurrentState = "KoopaShell" });
        entity.AddComponent(itemInput);

        return entity;
    }
}