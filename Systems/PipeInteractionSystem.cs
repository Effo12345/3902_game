
using ECS.Events;
using Microsoft.Xna.Framework;
using System;

namespace ECS.Systems;

public class PipeInteractionSystem : AbstractSystem
{
    private GameManager GameManager;
    // private string Destination = "";

    public PipeInteractionSystem(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        engine.PubSub.CreateSubscription<PipeEvent>(HandleEntry);
        // engine.PubSub.CreateSubscription<InputActionEvent>(HandleInput);
        return true;
    }
    private void HandleInput(IEvent evnt)
    {
        var inputEvent = (InputActionEvent)evnt;
        if (inputEvent.Action == "Interaction" && inputEvent.Phase == InputPhase.Press)
        {
            
        }
    }

    private void HandleEntry(IEvent evnt)
    {
        var pipeEvent = (PipeEvent)evnt;

        if (pipeEvent.CollisionDirection.Equals(pipeEvent.PipeDirection))
        {
            GameManager.ChangeLevel(pipeEvent.Destination);
        }
    }
    public override void Update(Engine engine, GameTime gameTime)
    {}
}