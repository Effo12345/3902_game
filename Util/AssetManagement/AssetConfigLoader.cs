namespace ECS.Util;

public class AssetFile
{
    public string Key { get; set; }
    public string Path { get; set; }
}

public class EntityFiles
{
    public string Key { get; set; }
    public string EntityConfig { get; set; }
    public string SpriteTexture { get; set; }
    public string SpriteAtlas { get; set; }
    public string InputConfig { get; set; }
}

public class AssetConfig
{
    public List<AssetFile> Textures { get; set; } = [];
    public List<AssetFile> Fonts { get; set; } = [];
    public List<AssetFile> Sounds { get; set; } = [];
    public List<AssetFile> SpriteAtlases { get; set; } = [];
    public List<AssetFile> InputConfigs { get; set; } = [];
    public List<AssetFile> EntityConfigs { get; set; } = [];
    public List<AssetFile> LevelConfigs { get; set; } = [];
    public List<EntityFiles> Entities { get; set; } = [];
}

public class AssetConfigLoader
{
    private static readonly JsonSerializerOptions LoadOptions = new()
    {
        // Ignore case since it'll just cause problems
        PropertyNameCaseInsensitive = true
    };

    // This probably needs a better name to not get confused with
    // dictionary TryGetValue
    private static T TryGetValue<T>(JsonElement elem, string property, T defaultValue)
    {
        if (!elem.TryGetProperty(property, out var prop))
            return defaultValue;

        try
        {
            return JsonSerializer.Deserialize<T>(prop.GetRawText(), LoadOptions);
        }
        catch (JsonException)
        {
            return defaultValue;
        }
    }

    public static AssetConfig Load(string configText)
    {
        var dom = JsonDocument.Parse(configText);
        var root = dom.RootElement;

        var assets = new AssetConfig
        {
            Textures = TryGetValue(root, "textures", new List<AssetFile>()),
            Fonts = TryGetValue(root, "fonts", new List<AssetFile>()),
            // Sounds = TryGetValue(root, "sounds", new List<AssetFile>()),
            SpriteAtlases = TryGetValue(root, "spriteAtlases", new List<AssetFile>()),
            InputConfigs = TryGetValue(root, "inputConfigs", new List<AssetFile>()),
            EntityConfigs = TryGetValue(root, "entityConfigs", new List<AssetFile>()),
            LevelConfigs = TryGetValue(root, "levelConfigs", new List<AssetFile>()),
            Entities = TryGetValue(root, "entities", new List<EntityFiles>())
        };

        return assets;
    }
}