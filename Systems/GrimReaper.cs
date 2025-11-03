
using System.ComponentModel;

namespace ECS.Systems;

public class MarioDefeatSystem : AbstractSystem
{
    public override void Update(Engine engine, GameTime gameTime)
    {
        foreach (var entity in engine.GetEntities())
        {
            if (!entity.TryGetComponent<Status>(out var status))
                continue;

            if (status.State == "dead")
            {
                //Remove unnecessary components
                entity.RemoveComponent<Collider>();
                entity.RemoveComponent<Friction>();
                entity.RemoveComponent<InputBindings>();

                //Apply upwards velocity
                if (entity.TryGetComponent<Velocity>(out var velocity))
                {
                    velocity.Value.Y = -500;
                    velocity.Value.X = 0;
                }
                
                entity.RemoveComponent<Status>();

            }
            
        }
    }
}