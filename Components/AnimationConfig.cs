namespace ECS.Components;

public struct AnimationConfig
{
    public Dictionary<string, AnimationDescription> States;
}

public struct AnimationDescription
{
    public List<Rectangle> Frames;
    public float FrameDuration;
}