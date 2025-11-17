
namespace ECS.Systems;

public class InvincibilitySystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status))
                continue;
            if (status.isInvincible)
                status.InvincibilityDuration -= deltaTime;
                if (status.InvincibilityDuration < 0)
                {
                    status.isInvincible = false;
                    status.InvincibilityDuration = CONSTANTS.TEMP.STAR_TIMER; //TODO: This should reset to a fixed value.
                }
        }
    }
}