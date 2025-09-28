
namespace ECS.Systems;

public class EnemyMovementSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(Engine);
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
        }
    }
}