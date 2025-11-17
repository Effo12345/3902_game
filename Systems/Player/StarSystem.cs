namespace ECS.Components;

public class StarSystem : AbstractSystem {



    public override void Update(Engine engine, GameTime gametime)
    {
        float deltaTime = (float)gametime.ElapsedGameTime.TotalSeconds;
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status))
                continue;

            if (status.StarTimer > 0)
            {
                status.StarTimer -= deltaTime;
                Console.WriteLine(status.StarTimer);
            }
            
        }
    }
}