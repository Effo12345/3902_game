namespace ECS.Systems;

public class InputSystem : AbstractSystem
{
    private KeyboardState _prevKeyboard;
    private MouseState _prevMouse;

    public override void Update(Engine engine, GameTime gameTime)
    {
        var kb = Keyboard.GetState();
        var ms = Mouse.GetState();

        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<InputBindings>(out var bindings))
                continue;

            // 1) Mouse input
            if (bindings.UsePointer)
            {
                var delta = new Point(ms.X - _prevMouse.X, ms.Y - _prevMouse.Y);
                var wheelDelta = ms.ScrollWheelValue - _prevMouse.ScrollWheelValue;

                if (delta != Point.Zero || wheelDelta != 0)
                {
                    engine.PubSub.Publish(new InputPointerEvent
                    {
                        Position = new Point(ms.X, ms.Y),
                        Delta = delta,
                        WheelDelta = wheelDelta,
                        Entity = entity
                    });
                }
            }

            // 2) Action Axis add up to Move Button sends Press/Hold/Release
            Vector2 velocitymove = Vector2.Zero;
            Vector2 accelerationmove = Vector2.Zero;

            foreach (var kv in bindings.Actions)
            {
                var actionName = kv.Key;
                var ab = kv.Value;

                if (ab.Type == InputActionType.Velocity)
                {
                    // Any pressed key or mousekey add up the binded vector
                    if ((ab.Keys != null && ab.Keys.Any(kb.IsKeyDown)) ||
                        (ab.MouseButtons != null && IsAnyMouseDown(ms, ab.MouseButtons)))
                    {
                        velocitymove += new Vector2(ab.AxisX, ab.AxisY);
                    }
                }
                else if (ab.Type == InputActionType.Acceleration)
                {
                    // Any pressed key or mousekey add up the binded vector
                    if ((ab.Keys != null && ab.Keys.Any(kb.IsKeyDown)) ||
                        (ab.MouseButtons != null && IsAnyMouseDown(ms, ab.MouseButtons)))
                    {
                        accelerationmove += new Vector2(ab.AxisX, ab.AxisY);
                    }
                }
                else // Button
                {
                    bool nowDown =
                        (ab.Keys != null && ab.Keys.Any(kb.IsKeyDown)) ||
                        (ab.MouseButtons != null && IsAnyMouseDown(ms, ab.MouseButtons));

                    bool prevDown =
                        (ab.Keys != null && ab.Keys.Any(_prevKeyboard.IsKeyDown)) ||
                        (ab.MouseButtons != null && IsAnyMouseDown(_prevMouse, ab.MouseButtons));

                    if (nowDown && !prevDown) PublishButton(engine, entity, actionName, InputPhase.Press);
                    if (nowDown && prevDown) PublishButton(engine, entity, actionName, InputPhase.Hold);
                    if (!nowDown && prevDown) PublishButton(engine, entity, actionName, InputPhase.Release);
                }
            }

            if (velocitymove != Vector2.Zero)
            {
                engine.PubSub.Publish(new InputAxisEvent
                {
                    Action = CONSTANTS.ACTION.VELOCITY,
                    Value = velocitymove,
                    Entity = entity
                });
            }

            if (accelerationmove != Vector2.Zero)
            {
                engine.PubSub.Publish(new InputAxisEvent
                {
                    Action = CONSTANTS.ACTION.ACCELERATION,
                    Value = accelerationmove,
                    Entity = entity
                });
            }
        }

        _prevKeyboard = kb;
        _prevMouse = ms;
    }

    private static void PublishButton(Engine engine, Entity entity, string action, InputPhase phase)
    {
        engine.PubSub.Publish(new InputActionEvent
        {
            Action = action,
            Phase = phase,
            Entity = entity
        });
    }

    // Check if any mouse button is pressed
    private static bool IsAnyMouseDown(MouseState ms, IEnumerable<string> buttons)
    {
        if (buttons == null) return false;

        foreach (var b in buttons)
        {
            // TODO: Murder the switch case
            switch (b)
            {
                case "Left": if (ms.LeftButton == ButtonState.Pressed) return true; break;
                case "Right": if (ms.RightButton == ButtonState.Pressed) return true; break;
                case "Middle": if (ms.MiddleButton == ButtonState.Pressed) return true; break;
                case "X1": if (ms.XButton1 == ButtonState.Pressed) return true; break;
                case "X2": if (ms.XButton2 == ButtonState.Pressed) return true; break;
            }
        }
        return false;
    }
}
