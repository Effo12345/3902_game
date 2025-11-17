namespace ECS.Util;

public class JsonStringColorConverter : JsonConverter<Color>
{
    private const int NUM_VALUES = 4;

    public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Invalid format for Color. Must be array notation");

        int[] values = new int[NUM_VALUES];
        int idx = 0;

        reader.Read();  // Clear array start

        while (idx < NUM_VALUES && reader.TokenType != JsonTokenType.EndArray)
        {
            values[idx] = reader.GetInt32();
            reader.Read();
            idx++;
        }

        return new Color(values[0], values[1], values[2], values[3]);
    }

    public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.R);
        writer.WriteNumberValue(value.G);
        writer.WriteNumberValue(value.B);
        writer.WriteNumberValue(value.A);
        writer.WriteEndArray();
    }
}