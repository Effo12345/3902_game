namespace ECS.Util;

public static class LevelLoader
{
    private static Assets Assets;

    public static void RegisterAssets(Assets assets)
    {
        Assets = assets;
    }

    public static void Load(Engine engine, string key)
    {
        engine.ClearEntities();

        var levelConfig = Assets.GetLevelConfig(key);
        foreach (var entityConfig in levelConfig.Entities)
        {
            var entity = EntityFactory.BuildEntity(engine, entityConfig.Key);
            EntityFactory.AppendComponentsFromConfig(entity, entityConfig.Config);
        }
        engine.PubSub.Publish(new LevelLoadedEvent{Level = key});
    }
}