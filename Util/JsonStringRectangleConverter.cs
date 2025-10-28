namespace ECS.Util;

public class JsonStringRectangleConverter : JsonConverter<Rectangle>
{
    public override Rectangle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Invalid format for Vector2. Must be array notation");

        reader.Read();
        int x = reader.GetInt32();

        reader.Read();
        int y = reader.GetInt32();

        reader.Read();
        int w = reader.GetInt32();

        reader.Read();
        int h = reader.GetInt32();

        reader.Read();  // Close array

        return new Rectangle(x, y, w, h);
    }

    public override void Write(Utf8JsonWriter writer, Rectangle value, JsonSerializerOptions options)
    {
        writer.WriteStartArray();
        writer.WriteNumberValue(value.X);
        writer.WriteNumberValue(value.Y);
        writer.WriteNumberValue(value.Width);
        writer.WriteNumberValue(value.Height);
        writer.WriteEndArray();
    }
}