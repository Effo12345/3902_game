namespace ECS.Util;

public static class SpriteSheetLoader
{
    private static readonly JsonSerializerOptions AnimOptions = new()
    {
        // Ignore case since it'll just cause problems
        PropertyNameCaseInsensitive = true
    };

    // Though not really needed, defining these as properties makes the annoying warnings go away
    private class InternalAnimation
    {
        public Dictionary<string, InternalAnimationConfig> Frames { get; set; }
    }

    private class InternalAnimationConfig
    {
        public float FrameDuration { get; set; }
        public List<InternalFrame> Frames { get; set; }
    }

    private class InternalFrame
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    // TODO: Do error handling here
    public static AnimationConfig Load(string configText)
    {
        var dom = JsonDocument.Parse(configText);
        var root = dom.RootElement;

        var deserializedData = root.Deserialize<InternalAnimation>(AnimOptions);

        AnimationConfig anim = new();
        anim.States = new();

        foreach (var (id, config) in deserializedData.Frames)
        {
            List<Rectangle> frames = new();
            foreach (var frame in config.Frames)
            {
                frames.Add(new Rectangle(frame.X, frame.Y, frame.Width, frame.Height));
            }

            anim.States.Add(id, new AnimationDescription { Frames = frames, FrameDuration = config.FrameDuration});
        }

        return anim;
    }
}