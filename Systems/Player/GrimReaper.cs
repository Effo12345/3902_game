
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
                !entity.TryGetComponent<Scale>(out var scale) ||
                animationStarted)
                continue;

            //If mario goes below a certain point, he dies
            if (position.Value.Y >= 500) //TODO: get rid of magic number and potentially relate to game window.
                status.State = CONSTANTS.PLAYER_STATE.DEAD;

            if (status.State == CONSTANTS.PLAYER_STATE.DEAD)
            {
                //Remove unnecessary components
                entity.RemoveComponent<Collider>();
                entity.RemoveComponent<Friction>();
                entity.RemoveComponent<InputBindings>();

                //Reset scale (for sunshine)
                scale.Value = new Vector2(3,3);

                //Apply upwards velocity
                velocity.Value.Y = CONSTANTS.COLLISION.MARIO_DEATH_VELOCITY;
                velocity.Value.X = 0;

                //Start timer for animation
                animationStarted = true;

                // Play death sound
                SoundPlayer.StopAll();
                SoundPlayer.PlaySfx(CONSTANTS.SOUND.DEATH);
            }
        }

        if (animationStarted)
        {
            timer += deltaTime;
            if (timer > CONSTANTS.TEMP.RESPAWN_TIMER)
            {
                timer = 0.0f;
                animationStarted = false;
                GameManager.ReloadCurrentLevel();
            }
        }
    }
}