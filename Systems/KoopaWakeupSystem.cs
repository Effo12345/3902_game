
namespace ECS.Systems;

public class KoopaWakeupSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<BasicEnemy>(out var enemy) ||
                !entity.TryGetComponent<Collider>(out var collider) ||
                !entity.TryGetComponent<Velocity>(out var velocity) ||
                !entity.TryGetComponent<FacingDirection>(out var facingDirection) ||
                !entity.TryGetComponent<SpriteState>(out var sprite) ||
                !entity.TryGetComponent<Position>(out var position))
                continue;

            if (collider.Type == ColliderType.Shell && Math.Abs(velocity.Value.X) < 0.001)
            {
                enemy.ShellTimer -= deltaTime;
                if (enemy.ShellTimer < 0)
                {
                    enemy.ShellTimer = 6;
                    collider.Type = ColliderType.Koopa;
                    velocity.Value.X = -50;
                    facingDirection.IsFacingLeft = true;
                    engine.PubSub.Publish(new AnimationChangeEvent
                    {
                        NewState = "koopa",
                        Entity = entity
                    });
                    position.Value.Y -= 34; //TODO: This is hard coded in to get koopa back on the ground when it wakes up
                }
            }
            else
                enemy.ShellTimer = 6;
        }
    }
}