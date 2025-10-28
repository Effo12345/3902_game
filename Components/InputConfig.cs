namespace ECS.Components;

public enum InputActionType { Button, Axis }
public enum InputPhase { Press, Hold, Release }

public sealed class InputBindings
{
    public Dictionary<string, ActionBinding> Actions = new(StringComparer.OrdinalIgnoreCase);
    public bool UsePointer = true; // Flag that determines whether listen to mouse/scroll
}

public sealed class ActionBinding
{
    public InputActionType Type;
    public List<Keys> Keys = new();          // Keyboard (could be multiple)
    public List<string> MouseButtons = new(); // "Left","Right","Middle","X1","X2"
    public float AxisX;                      // Countinuous axis input
    public float AxisY;
}