namespace ECS.Util;

// TODO: Clean up the warnings in this file
public static class InputDefinitionLoader
{
    // JSON ignore the capitalization
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true
    };

    // DTO
    private sealed class InternalBinding
    {
        public string Type { get; set; } = "Button";       // Button | Axis
        public List<string>? Keys { get; set; }            // ["W","A","Space"]
        public List<string>? MouseButtons { get; set; }    // Mousebutton["Left","Right"]
        public float AxisX { get; set; } = 0;              // Only Axis
        public float AxisY { get; set; } = 0;              // Only Axis
    }

    public static InputBindings LoadFromJson(string json)
    {
        var dict = JsonSerializer.Deserialize<Dictionary<string, InternalBinding>>(json, Options)
                   ?? new Dictionary<string, InternalBinding>(StringComparer.OrdinalIgnoreCase);

        var result = new InputBindings();

        foreach (var (actionName, ib) in dict)
        {
            var binding = new ActionBinding
            {
                Type = ParseType(ib.Type),
                Keys = ParseKeys(ib.Keys),
                MouseButtons = ib.MouseButtons ?? new List<string>(),
                AxisX = ib.AxisX,
                AxisY = ib.AxisY
            };

            result.Actions[actionName] = binding;
        }

        return result;
    }

    public static InputBindings LoadFromFile(string path)
    {
        var json = File.ReadAllText(path);
        return LoadFromJson(json);
    }

    // ---- Helper ----

    private static InputActionType ParseType(string? type)
    {
        if (type == null) return InputActionType.Button;
        return type.Equals("Axis", StringComparison.OrdinalIgnoreCase)
            ? InputActionType.Axis
            : InputActionType.Button;
    }

    private static List<Keys> ParseKeys(IEnumerable<string>? names)
    {
        var list = new List<Keys>();
        if (names == null) return list;

        foreach (var s in names)
        {
            if (Enum.TryParse<Keys>(s, ignoreCase: true, out var key))
                list.Add(key);
            // else: Ignore unrecognized keys
        }
        return list;
    }
}