
using System.Collections;
using System.Linq;

namespace ECS.Systems;

public class SpriteSwitchingSystem : AbstractSystem
{
    private enum enemyType {goomba, koopa};

    public override void Update(Engine engine, GameTime gameTime)
    {
        Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();
        foreach (var entity in Engine.GetEntities())
        {
            if (!entity.TryGetComponent<SpriteState>(out var sprite))
                continue;

            if (entity.TryGetComponent <BasicEnemy>(out var enemy)) //This is yuck my bad yall
            {
                foreach (var key in pressedKeys)
                {
                    if (key == Keys.O) //If O is pressed switch to Goomba
                    {
                        entity.RemoveComponent<Position>();
                        if (enemy.Name.Equals("Goomba"))
                        {
                            entity.AddComponent<Position>(new Position { Value = new Vector2(100, 100) });
                            enemy.Value = new Vector2(-100, 0);
                        }
                    }
                    if (key == Keys.P) //If P is pressed switch to Koopa
                    {
                        entity.RemoveComponent<Position>();
                        if (enemy.Name.Equals("Koopa"))
                        {
                            entity.AddComponent<Position>(new Position { Value = new Vector2(100, 100) });
                            enemy.Value = new Vector2(-100, 0);
                        }
                    }
                }
            }

            Engine.PubSub.Publish(new InputEvent
            {
                Entity = entity
            });
        }
    }
}