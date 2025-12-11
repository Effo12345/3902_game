namespace ECS.Components;

using Microsoft.Xna.Framework.Input;
using System.Diagnostics.Contracts;

public enum InputActionType { Button, Velocity, Acceleration}
public enum InputPhase { Press, Hold, Release }

public enum GamepadAxisSource
{
    None,
    LeftStick,
    RightStick
}
public sealed class InputBindings
{
    public Dictionary<string, ActionBinding> Actions = new(StringComparer.OrdinalIgnoreCase);
    public bool UsePointer = true; // Flag that determines whether listen to mouse/scroll
    public bool UseGamepad = true;
}

public sealed class ActionBinding
{
    public InputActionType Type;
    public List<Keys> Keys = new();          // Keyboard (could be multiple)
    public List<string> MouseButtons = new(); // "Left","Right","Middle","X1","X2"
    public List<Buttons> GamepadButtons = new();

    public float AxisX;                      // Countinuous axis input
    public float AxisY;
}