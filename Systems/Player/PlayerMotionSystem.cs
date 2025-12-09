using ECS.Core;
using ECS.Util;
using Microsoft.VisualBasic;

namespace ECS.Systems;

public class PlayerMotionSystem : AbstractSystem
{
    private int StateIndex = 0;
    private float FireballTimer = -1;
    private float HoverTimer = -1;


    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputAxisEvent>(OnAxis);
        engine.PubSub.CreateSubscription<InputActionEvent>(OnAction);
        engine.PubSub.CreateSubscription<InputGamepadStickEvent>(OnGamepad);
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

    private void OnGamepad(IEvent evt)
    {
        var e = (InputGamepadStickEvent)evt;
        if (e.Stick != GamepadStick.Left)
            return;
        var entity = e.Entity;

        if (!entity.TryGetComponent<Velocity>(out var velocity) ||
            !entity.TryGetComponent<IsGrounded>(out var grounded))
            return;

        var v = new Vector2(e.Value.X * 10.0f, 0f);

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
        else if (inputEvent.Action == CONSTANTS.ACTION.SPECIAL)
        {
            if (currentStatus.State == CONSTANTS.PLAYER_STATE.FIRE && inputEvent.Phase == InputPhase.Press && FireballTimer < 0) //Launch Fireball
            {
                FireballTimer  = .25f;
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

                    f_position.Value = position.Value + CONSTANTS.TEMP.FIREBALL_SPAWN;
                    f_velocity.Value *= direction;
                    SoundPlayer.PlaySfx(CONSTANTS.SOUND.FIREBALL);
                });
            }
            else if (currentStatus.State == CONSTANTS.PLAYER_STATE.SUNSHINE)
            {
                if (currentStatus.canHover && inputEvent.Phase == InputPhase.Press)
                {
                    HoverTimer = 3.0f; 
                }
                if (inputEvent.Phase == InputPhase.Hold && HoverTimer > 0) {
                    velocity.Value.Y = -50;
                    Engine.Defer(Engine =>
                    {
                        var fireball = EntityFactory.BuildEntity(Engine, "fireball"); //"water"
                        fireball.TryGetComponent<Position>(out var f_position);
                        fireball.TryGetComponent<Velocity>(out var f_velocity);
                        fireball.TryGetComponent<SpriteState>(out var f_spriteState);
                        fireball.TryGetComponent<Collider>(out var collider);

                        f_position.Value = position.Value;
                        if (facingDirection.IsFacingLeft)
                            f_position.Value.X += 40;
                        f_velocity.Value.X = velocity.Value.X;
                        collider.Type = ColliderType.Water;

                        f_spriteState.Color = Color.Aqua;
                        SoundPlayer.PlaySfx(CONSTANTS.SOUND.MARIO_HOVER);
                    });
                }
                if (inputEvent.Phase == InputPhase.Release)
                {
                    SoundPlayer.Stop(CONSTANTS.SOUND.MARIO_HOVER);
                    currentStatus.canHover = false;
                    HoverTimer = -1.0f;
                }
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

    public override void Update(Engine engine, GameTime gameTime)
    {
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        if (FireballTimer > 0)
            FireballTimer -= deltaTime;
        if (HoverTimer > 0)
            HoverTimer -= deltaTime;

        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status) ||
                !entity.TryGetComponent<IsGrounded>(out var grounded))
                continue;

            if (grounded.Value)
            {
                status.canHover = true;
            }
        }

    }
}