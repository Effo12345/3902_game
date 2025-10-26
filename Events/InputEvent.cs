namespace ECS.Events;

//public struct InputEvent : IEvent
//{
//    public Vector2 MotionVector;
//    public Entity Entity;
//}

public sealed class InputActionEvent : IEvent
{
    public string Action = "";   // "Jump" "Fire" "Restart"
    public InputPhase Phase;     // Started/Performed/Canceled
    public Entity Entity;
}

public sealed class InputAxisEvent : IEvent
{
    public string Action = "";   // Such as "Move"
    public Vector2 Value;        // Combined 2D vector(-1..1)
    public Entity Entity;
}

public sealed class InputPointerEvent : IEvent
{
    public Point Position;   // Screen Pixel
    public Point Delta;      // Offset in current frame
    public int WheelDelta;   // Wheel scroll
    public Entity Entity;
}