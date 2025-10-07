namespace ECS.Util;

//This class is used to quickly and easily create an enemy with some set properties: Default speed -50 left, starts facing left, scaled 3x3, no acceleration, no rotation
//and animationConfig conventional for a goomba or a koopa.
public static class EnemyFactory
{
    private static Entity entity;

    //This method assumes some naming conventions in the animation config file, namely that it will have a state called the name of the enemy.
    public static Entity MakeEnemy(Engine engine, ContentManager Content, String name, Vector2 position, Vector2 velocity)
    {
        entity = engine.CreateEntity();
        var enemySprites = Content.Load<Texture2D>("Sprites/enemies");
        var enemyAnimationConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/enemy_spritesheet.json"));

        entity.AddComponent(new Position { Value = position });
        entity.AddComponent(new Velocity { Value = velocity });
        entity.AddComponent(new Acceleration {});
        entity.AddComponent(new BasicEnemy {});
        entity.AddComponent(new FacingDirection { IsFacingLeft = true });
        entity.AddComponent(new Rotation { Value = 0.0f });
        entity.AddComponent(new Scale { Value = new Vector2(3, 3) });
        entity.AddComponent(new SpriteState { Texture = enemySprites, Color = Color.White });
        entity.AddComponent(new AnimationConfig { States = enemyAnimationConfig.States });
        entity.AddComponent(new AnimationState { CurrentState = name });
        entity.AddComponent(InputDefinitionLoader.Load(File.ReadAllText("Content/Config/enemy_input.json")));

        return entity;
    }
}