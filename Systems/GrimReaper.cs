
using System.ComponentModel;

namespace ECS.Systems;

public class MarioDefeatSystem : AbstractSystem
{
    private GameManager GameManager;
    private bool animationStarted = false;
    private float timer = 0.0f;
    public MarioDefeatSystem(GameManager gameManager)
    {
        GameManager = gameManager;
    }
    public override void Update(Engine engine, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status) ||
                !entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<Position>(out var position) ||
                animationStarted)
                continue;

            //If mario goes below a certain point, he dies
            if (position.Value.Y >= 500) //TODO: get rid of magic number and potentially relate to game window.
                status.State = "dead";

            if (status.State == "dead")
            {
                //Remove unnecessary components
                entity.RemoveComponent<Collider>();
                entity.RemoveComponent<Friction>();
                entity.RemoveComponent<InputBindings>();

                //Apply upwards velocity
                velocity.Value.Y = -500;
                velocity.Value.X = 0;

                //Start timer for animation
                animationStarted = true;
            }
        }

        if (animationStarted)
        {
            timer += deltaTime;
            if (timer > 3.0)
            {
                timer = 0.0f;
                animationStarted = false;
                //TODO: play sound for mario dying here
                GameManager.ChangeLevel("Mario1_1");
            }
        }
    }
}