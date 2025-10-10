using ECS.Core;
using Microsoft.Xna.Framework.Input;

namespace ECS.Systems;

public class MarioActionSystem : AbstractSystem
{
    private Entity Mario;
    private int StateIndex = 0;

    public MarioActionSystem(Entity mario)
    {
        Mario = mario;
    }
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<InputActionEvent>(OnAction);
        return true;
    }
    private void OnAction(IEvent evt)
    {
        var inputEvent = (InputActionEvent)evt;
        var entity = inputEvent.Entity;

        if (!Mario.TryGetComponent<Status>(out var currentStatus) ||
                    !Mario.TryGetComponent<Position>(out var position) ||
                    !Mario.TryGetComponent<FacingDirection>(out var facingDirection)
                    )
            return;

        if (inputEvent.Action == "SwitchState" && inputEvent.Phase == InputPhase.Started)
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
        else if (inputEvent.Action == "Fire" && inputEvent.Phase == InputPhase.Started)
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
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
    }
}