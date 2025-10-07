namespace ECS.Components;

public class AnimationConfig
{
    public Dictionary<string, AnimationDescription> States;
}

public class AnimationDescription
{
    public List<Rectangle> Frames;
    public float FrameDuration;
}