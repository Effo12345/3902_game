namespace ECS.Systems;

public class InputSystem : AbstractSystem
{
    private KeyboardState _prevKeyboard;
    private MouseState _prevMouse;
    private GamePadState _prevGamepad;
    public override void Update(Engine engine, GameTime gameTime)
    {
        var kb = Keyboard.GetState();
        var ms = Mouse.GetState();
        var gp = GamePad.GetState(PlayerIndex.One);

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
                    bool down =
                        (ab.Keys != null && ab.Keys.Any(kb.IsKeyDown)) ||
                        (ab.MouseButtons != null && IsAnyMouseDown(ms, ab.MouseButtons)) ||
                        (ab.GamepadButtons != null && IsAnyGamepadDown(gp, ab.GamepadButtons));

                    if (down)
                    {
                        velocitymove += new Vector2(ab.AxisX, ab.AxisY);
                    }
                }
                else if (ab.Type == InputActionType.Acceleration)
                {
                    bool down =
                        (ab.Keys != null && ab.Keys.Any(kb.IsKeyDown)) ||
                        (ab.MouseButtons != null && IsAnyMouseDown(ms, ab.MouseButtons)) ||
                        (ab.GamepadButtons != null && IsAnyGamepadDown(gp, ab.GamepadButtons));

                    if (down)
                    {
                        accelerationmove += new Vector2(ab.AxisX, ab.AxisY);
                    }
                }
                else // Button
                {
                    bool nowDown =
                        (ab.Keys != null && ab.Keys.Any(kb.IsKeyDown)) ||
                        (ab.MouseButtons != null && IsAnyMouseDown(ms, ab.MouseButtons)) ||
                        (ab.GamepadButtons != null && IsAnyGamepadDown(gp, ab.GamepadButtons));

                    bool prevDown =
                        (ab.Keys != null && ab.Keys.Any(_prevKeyboard.IsKeyDown)) ||
                        (ab.MouseButtons != null && IsAnyMouseDown(_prevMouse, ab.MouseButtons)) ||
                        (ab.GamepadButtons != null && IsAnyGamepadDown(_prevGamepad, ab.GamepadButtons));


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

            if (bindings.UseGamepad)
            {
                var prevLeftRaw = _prevGamepad.ThumbSticks.Left;
                var prevRightRaw = _prevGamepad.ThumbSticks.Right;
                var currLeftRaw = gp.ThumbSticks.Left;
                var currRightRaw = gp.ThumbSticks.Right;

                var prevLeft = new Vector2(prevLeftRaw.X, -prevLeftRaw.Y);
                var currLeft = new Vector2(currLeftRaw.X, -currLeftRaw.Y);
                var prevRight = new Vector2(prevRightRaw.X, -prevRightRaw.Y);
                var currRight = new Vector2(currRightRaw.X, -currRightRaw.Y);

                if (bindings.UseGamepad)
                {
                    var deltaL = currLeft - prevLeft;
                    if (currLeft != Vector2.Zero)
                    {
                        engine.PubSub.Publish(new InputGamepadStickEvent
                        {
                            Stick = GamepadStick.Left,
                            Value = currLeft,
                            Delta = deltaL,
                            Entity = entity
                        });
                    }

                    var deltaR = currRight - prevRight;
                    if (currRight != Vector2.Zero)
                    {
                        engine.PubSub.Publish(new InputGamepadStickEvent
                        {
                            Stick = GamepadStick.Right,
                            Value = currRight,
                            Delta = deltaR,
                            Entity = entity
                        });
                    }
                }
            }
        }

        _prevKeyboard = kb;
        _prevMouse = ms;
        _prevGamepad = gp;
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

    private static bool IsAnyGamepadDown(GamePadState gp, IEnumerable<Buttons>? buttons)
    {
        if (buttons == null) return false;

        foreach (var b in buttons)
        {
            if (gp.IsButtonDown(b))
                return true;
        }
        return false;
    }

}
