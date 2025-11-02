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
            !entity.TryGetComponent<FacingDirection>(out var facingDirection) ||
            !entity.TryGetComponent<IsGrounded>(out var grounded)
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

                Engine.Defer(Engine =>
                {
                    var fireball = EntityFactory.BuildEntity(Engine, "fireball");
                    fireball.TryGetComponent<Position>(out var f_position);
                    fireball.TryGetComponent<Velocity>(out var f_velocity);

                    f_position.Value = position.Value;
                    f_velocity.Value = direction * 250f;

                    //var fireball = Engine.CreateEntity();
                    ////var itemsprite = Assets.GetTexture("ItemsSheet");
                    //// var fireballsprite = Content.Load<Texture2D>("Sprites/items");
                    //var itemConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/items_spritesheet.json"));

                    //// TODO: Fix the spawn position for fireball
                    //fireball.AddComponent(new Position { Value = initialPosition });
                    //fireball.AddComponent(new Rotation { Value = 0f });
                    //fireball.AddComponent(new Scale { Value = new Vector2(1.5f, 1.5f) });
                    //fireball.AddComponent(new Velocity { Value = direction * 250f });
                    //fireball.AddComponent(new VelocityCap());
                    //fireball.AddComponent(new Acceleration());
                    //fireball.AddComponent(new Mass { Value = 2.5f });
                    ////fireball.AddComponent(new SpriteState { Texture = itemsprite, SourceRect = itemConfig.States["Fireball"].Frames[0], Color = Color.White });
                    //fireball.AddComponent(new AnimationConfig { States = itemConfig.States });
                    //fireball.AddComponent(new AnimationState { CurrentState = "Fireball" });
                });

                //Engine.PubSub.Publish(new FireballLaunch
                //{
                //    Position = position,
                //    Direction = direction
                //});
            }
        }
        else if (inputEvent.Action == "Jump")
        {
            // TODO: Make holding the button cause higher jumps
            if (inputEvent.Phase == InputPhase.Press && grounded.Value)
                velocity.Value.Y += -300.0f; // TODO: Move this back to config file
        }
    }

    // no-op
    public override void Update(Engine engine, GameTime gameTime) { }
}