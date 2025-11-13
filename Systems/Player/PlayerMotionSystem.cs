using ECS.Core;
using ECS.Util;

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
        if (e.Action != CONSTANTS.ACTION.VELOCITY) return;
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
            !entity.TryGetComponent<FacingDirection>(out var facingDirection) ||
            !entity.TryGetComponent<IsGrounded>(out var grounded)
        )
            return;

        if (inputEvent.Action == "SwitchState" && inputEvent.Phase == InputPhase.Press)
        {
            StateIndex++;
            if (StateIndex % 3 == 0)
            {
                currentStatus.State = CONSTANTS.PLAYER_STATE.SMALL;
            }
            else if (StateIndex % 3 == 1)
            {
                currentStatus.State = CONSTANTS.PLAYER_STATE.BIG;
            }
            else if (StateIndex % 3 == 2)
            {
                currentStatus.State = CONSTANTS.PLAYER_STATE.FIRE;
            }
        }
        else if (inputEvent.Action == CONSTANTS.ACTION.FIRE && inputEvent.Phase == InputPhase.Press)
        {
            if (currentStatus.State == CONSTANTS.PLAYER_STATE.FIRE)
            {
                Vector2 direction;
                if (facingDirection.IsFacingLeft)
                    direction = new Vector2(-1.0f, 0.0f);
                else
                    direction = new Vector2(1.0f, 0.0f);

                Vector2 initialPosition = position.Value + new Vector2(20.0f, 50.0f);

                Engine.Defer(Engine =>
                {
                    var fireball = EntityFactory.BuildEntity(Engine, "fireball");
                    fireball.TryGetComponent<Position>(out var f_position);
                    fireball.TryGetComponent<Velocity>(out var f_velocity);

                    f_position.Value = position.Value;
                    f_velocity.Value = direction * 250f;
                });
            }
        }
        else if (inputEvent.Action == CONSTANTS.ACTION.JUMP)
        {
            // TODO: Make holding the button cause higher jumps
            if (inputEvent.Phase == InputPhase.Press && grounded.Value)
            {
                velocity.Value.Y += CONSTANTS.TEMP.JUMP_VELOCITY; // TODO: Move this back to config file
                SoundPlayer.PlaySfx(CONSTANTS.SOUND.JUMP);
            }
        }
    }

    // no-op
    public override void Update(Engine engine, GameTime gameTime) { }
}