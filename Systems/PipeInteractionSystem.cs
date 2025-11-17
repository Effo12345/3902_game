
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

    private void HandleEntry(IEvent e)
    {
        Console.WriteLine("Event");
        var pipeEvent = (PipeEvent)e;
 
        Direction inputDirection = pipeEvent.InputDirection;
        Direction pipeDirection = pipeEvent.PipeDirection;
        Entity entity = pipeEvent.Entity;
        Vector2 destination = pipeEvent.Destination;

        if (inputDirection.Equals(pipeDirection))
        {
           if (entity.TryGetComponent<Position>(out var position)) {
                position.Value = destination;
            }
        }
    }
    public override void Update(Engine engine, GameTime gameTime)
    {}
}