namespace ECS.Util;

public static class TileEntityFactory
{
    public static Entity MakeBlock(TileEntityConfigData config, string blockType)
    {
        Entity entity = config.Engine.CreateEntity();
        var collisionBox = new Rectangle((int)config.Pos.X, (int)config.Pos.Y, config.Animation.States["block"].Frames[0].Width * 3, config.Animation.States["block"].Frames[0].Height * 3); //TODO: make scale not a magic number

        entity.AddComponent(new Position { Value = config.Pos });
        entity.AddComponent(new CollisionInfo { HitBox = collisionBox, CollisionType = CollisionType.nonMovingBoxCollider });
        entity.AddComponent(new Rotation { Value = 0f });
        entity.AddComponent(new Scale { Value = new Vector2(3, 3) });

        // Lots of accessors here
        entity.AddComponent(new SpriteState
        {
            Texture = config.Texture,
            SourceRect = config.Animation.States[blockType].Frames[0],
            Origin = Vector2.Zero,
            Color = Color.White
        });

        return entity;
    }

    public static Entity MakeBumpBlock(TileEntityConfigData config, string blockType)
    {
        Entity entity = MakeBlock(config, blockType);
        entity.AddComponent(new Bump());

        return entity;
    }

    public static Entity MakeBreakBlock(TileEntityConfigData config, string blockType)
    {
        Entity entity = MakeBlock(config, blockType);
        entity.AddComponent(new Break());

        return entity;
    }

    public static Entity MakeItemBlock(TileEntityConfigData config, string blockType, ItemKind kind, string itemState, string usedBlockState)
    {
        Entity entity = MakeBlock(config, blockType);
        entity.AddComponent(new Bump());

        entity.AddComponent(new ItemBlock
        {
            Kind = kind,
            ItemTexture = config.Texture,
            ItemRect = config.Animation.States[itemState].Frames[0],
            UsedBlockRect = config.Animation.States[usedBlockState].Frames[0],
            Used = false
        });

        return entity;
    }

    public static Entity MakeCoin(TileEntityConfigData config)
    {
        Entity entity = config.Engine.CreateEntity();

        entity.AddComponent(new Position { Value = config.Pos });
        entity.AddComponent(new Rotation { Value = 0f });
        entity.AddComponent(new Scale { Value = new Vector2(2, 2) });
        entity.AddComponent(new SpriteState
        {
            Texture = config.Texture,
            Origin = Vector2.Zero,
            Color = Color.White
        });
        entity.AddComponent(config.Animation);
        entity.AddComponent(new AnimationState
        {
            CurrentState = "Coin",
            FrameIndex = 0,
            TimeInCurrentFrame = 0f
        });

        return entity;
    }
}

public class TileEntityConfigData
{
    public Engine Engine;
    public Vector2 Pos;
    public Texture2D Texture;
    public AnimationConfig Animation;
}