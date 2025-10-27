namespace ECS.Util;

public class LevelEntity
{
    public string Key { get; set; }
    public EntityConfig Config { get; set; }
}

public class LevelConfig
{
    public List<LevelEntity> Entities { get; set; } = [];
}


public static class LevelLoader
{
    private class JsonEntity
    {
        public string Key { get; set; }
        public JsonElement Components { get; set; }
    }

    private class JsonLevel
    {
        public List<JsonEntity> Entities { get; set; } = [];
    }

    private static readonly JsonSerializerOptions LevelOptions = new()
    {
        // Ignore case since it'll just cause problems
        PropertyNameCaseInsensitive = true
    };

    public static LevelConfig Load(string configText)
    {
        var dom = JsonDocument.Parse(configText);
        var root = dom.RootElement;

        var conf = new LevelConfig();

        if (!root.TryGetProperty("entities", out var entities))
            return conf;

        // JsonSerializer.Deserialize<List<LevelEntity>>(entities.GetRawText(), LevelOptions);

        var jsonLevel = root.Deserialize<JsonLevel>(LevelOptions);

        foreach (var entityConfig in jsonLevel.Entities)
        {
            Console.WriteLine(entityConfig.Components);

            conf.Entities.Add(new LevelEntity
            {
                Key = entityConfig.Key,
                Config = EntityLoader.Load(entityConfig.Components.GetRawText())
            });
        }

        return conf;
    }
}