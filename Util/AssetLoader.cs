namespace ECS.Util;

public static class AssetLoader
{
    public static Assets Load(ContentManager content, string configText)
    {
        var assets = new Assets();
        var config = AssetConfigLoader.Load(configText);

        LoadAssetType(config.Textures, (key, path) => assets.LoadTexture(content, key, path));
        LoadAssetType(config.SpriteAtlases, assets.LoadSpriteAtlas);
        LoadAssetType(config.InputConfigs, assets.LoadInputConfig);
        LoadAssetType(config.LevelConfigs, assets.LoadLevelConfig);
        LoadAssetType(config.EntityConfigs, assets.LoadEntityConfig);

        RegisterEntities(config.Entities);

        return assets;
    }

    private static void LoadAssetType(List<AssetFile> assetFiles, Action<string, string> loadCall)
    {
        foreach (var asset in assetFiles)
            loadCall(asset.Key, asset.Path);
    }

    private static void RegisterEntities(List<EntityFiles> entities)
    {
        foreach (var entity in entities)
        {
            EntityFactory.RegisterEntity(
                new EntityFiles
                {
                    Key = entity.Key,
                    EntityConfig = string.IsNullOrEmpty(entity.EntityConfig) ? null : entity.EntityConfig,
                    SpriteTexture = string.IsNullOrEmpty(entity.SpriteTexture) ? null : entity.SpriteTexture,
                    SpriteAtlas = string.IsNullOrEmpty(entity.SpriteAtlas) ? null : entity.SpriteAtlas,
                    InputConfig = string.IsNullOrEmpty(entity.InputConfig) ? null : entity.InputConfig
                }
            );
        }
    }
}