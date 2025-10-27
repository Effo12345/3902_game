
using ECS.Components;
using Microsoft.Xna.Framework;
using System;

namespace ECS.Systems;

public class PlayerMotionSystem : AbstractSystem
{
    private int StateIndex = 0;
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputAxisEvent>(OnAxis);
        engine.PubSub.CreateSubscription<InputActionEvent>(OnAction);
        return true;
    }
    private void OnAxis(IEvent evt)
    {
        var e = (InputAxisEvent)evt;
        if (e.Action != "Move") return;
        var entity = e.Entity;

        if (!entity.TryGetComponent<Velocity>(out var velocity) ||
            !entity.TryGetComponent<IsGrounded>(out var grounded))
            return;

        var v = e.Value;
        if (!grounded.Value) v.Y = 0f;
        velocity.Value += v;
    }
    private void OnAction(IEvent evt)
    {
        var inputEvent = (InputActionEvent)evt;
        var entity = inputEvent.Entity;

        if (!entity.TryGetComponent<Status>(out var currentStatus) ||
                    !entity.TryGetComponent<Position>(out var position) ||
                    !entity.TryGetComponent<Velocity>(out var velocity) ||
                    !entity.TryGetComponent<FacingDirection>(out var facingDirection)
                    )
            return;

        if (inputEvent.Action == "SwitchState" && inputEvent.Phase == InputPhase.Press)
        {
            StateIndex++;
            if (StateIndex % 3 == 0)
            {
                currentStatus.State = "small";
            }
            else if (StateIndex % 3 == 1)
            {
                currentStatus.State = "big";
            }
            else if (StateIndex % 3 == 2)
            {
                currentStatus.State = "fire";
            }
        }
        else if (inputEvent.Action == "Fire" && inputEvent.Phase == InputPhase.Press)
        {
            if (currentStatus.State == "fire")
            {
                Vector2 direction;
                if (facingDirection.IsFacingLeft)
                    direction = new Vector2(-1.0f, 0.0f);
                else
                    direction = new Vector2(1.0f, 0.0f);

                Vector2 initialPosition = position.Value + new Vector2(20.0f, 50.0f);


                Engine.PubSub.Publish(new FireballLaunch
                {
                    Position = position,
                    Direction = direction
                });
            }
        }
        else if (inputEvent.Action == "Jump")
        {
            switch (inputEvent.Phase)
            {
                case InputPhase.Press:
                    velocity.Value.Y += -200.0f;
                    break;
                case InputPhase.Hold:
                    break;
                case InputPhase.Release:
                    break;
            }
        }
    }

    // no-op
    public override void Update(Engine engine, GameTime gameTime) { }
}