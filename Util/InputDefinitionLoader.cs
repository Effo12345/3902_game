namespace ECS.Util;

// TODO: Make an abstract jsonLoader class
public static class InputDefinitionLoader
{
    private static readonly JsonSerializerOptions AnimOptions = new()
    {
        // Ignore case since it'll just cause problems
        PropertyNameCaseInsensitive = true
    };

    private class InternalInputAction
    {
        public List<string> Keys { get; set; }
        public float XVel { get; set; }
        public float YVel { get; set; }
    }

    // TODO: Do error handling here
    public static InputConfig Load(string configText)
    {
        var dom = JsonDocument.Parse(configText);
        var root = dom.RootElement;

        var deserializedData = root.Deserialize<Dictionary<string, InternalInputAction>>(AnimOptions);

        InputConfig config = new();
        config.Actions = new();

        foreach (var action in deserializedData.Values)
        {
            InputAction input = new InputAction
            {
                Keys = [.. action.Keys.Select(Enum.Parse<Keys>)],
                Velocity = new Vector2(action.XVel, action.YVel)
            };

            config.Actions.Add(input);
        }

        return config;
    }
}