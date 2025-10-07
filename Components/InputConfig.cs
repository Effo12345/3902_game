namespace ECS.Components;

public class InputConfig
{
    public List<InputAction> Actions;
}

public class InputAction
{
    public List<Keys> Keys;
    public Vector2 Velocity;
}