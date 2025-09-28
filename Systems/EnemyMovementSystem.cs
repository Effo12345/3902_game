
namespace ECS.Systems;

public class EnemyMovementSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        return true;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        // This line is important! Any time-based stuff needs to be relative to deltatime
        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        //For every entity with the BasicEnemyMovement component, move them as follows:
        foreach (var entity in Engine.GetEntities())
        {
            //Entity needs BasicEnemyMovement as well as Position to be affected
            if (!entity.TryGetComponent<Position>(out var position) ||
                !entity.TryGetComponent<BasicEnemyMovement>(out var basicEnemyMovement)
            )
                continue;

            //If the enemy collides with something it cannot move into, change its direction and flip its sprite.
            if (position.Value.X <= 0 || position.Value.X >= 752)
            {
                basicEnemyMovement.Value.X *= -1;
            }

            //TODO: Enemies can fall in SMB1. For sprint2 theres no ground, and no good way to implement that, so this section is reserved for making enemies fall later on

            //Move enemy according to its velocity
            position.Value += basicEnemyMovement.Value * deltaTime;
        }
    }
}