namespace ECS.Util;

public class EntityConfig
{
    public Dictionary<Type, object> Components { get; } = [];
}

public static class EntityLoader
{
    private static readonly Dictionary<string, Type> ValidComponents;
    private static readonly JsonSerializerOptions JsonOptions;

    static EntityLoader()
    {
        // TODO: Can this be done at compile time?
        ValidComponents = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())  // Flatten types from loaded assemblies
            .Where(t =>
                t.Namespace?.StartsWith("ECS.Components") == true
            )
            .ToDictionary(
                t => t.Name,                      // Type string as key
                t => t,                           // Actual type as value
                StringComparer.OrdinalIgnoreCase  // Ignore case
            );

        JsonOptions = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            Converters =
            {
                new JsonStringEnumConverter(),
                new JsonStringVector2Converter(),
                new JsonStringRectangleConverter(),
                new JsonStringColorConverter()
            }
        };
    }

    public static EntityConfig Load(string configText)
    {
        var dom = JsonDocument.Parse(configText);
        var root = dom.RootElement;

        var entityConfig = new EntityConfig();

        foreach (var componentJson in root.EnumerateObject())
        {
            string componentName = componentJson.Name;

            if (!ValidComponents.TryGetValue(componentName, out Type componentType))
                throw new InvalidDataException($"Invalid component type specified: {componentName}");

            var component = JsonSerializer.Deserialize(
                componentJson.Value.GetRawText(),
                componentType,
                JsonOptions
            );

            entityConfig.Components[componentType] = component;
        }

        return entityConfig;
    }
}