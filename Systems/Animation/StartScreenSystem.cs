
namespace ECS.Systems;

public class StartScreenSystem : AbstractSystem
{
    GameManager GameManager;

    public StartScreenSystem(GameManager gameManager)
    {
        GameManager = gameManager;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        var deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<StartScreenPause>(out var pause) ||
                !entity.TryGetComponent<Position>(out var position)
            )
                continue;

            engine.PubSub.Publish(new CameraLockEvent { DoLockCamera = true, LockPos = position.Value });

            if (pause.Duration <= 0.0f) {
                engine.PubSub.Publish(new CameraLockEvent { DoLockCamera = false, LockPos = Vector2.Zero });
                GameManager.LoadNextRequestedLevel();
            }

            pause.Duration -= deltaTime;
        }
    }
}