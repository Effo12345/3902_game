namespace ECS.Util;

public static class EntityFactory
{
    private static Dictionary<string, EntityFiles> Entities = [];
    private static Assets Assets;

    public static void RegisterEntity(EntityFiles entity)
    {
        Entities[entity.Key] = entity;
    }

    public static void RegisterAssets(Assets assets)
    {
        Assets = assets;
    }

    public static Entity BuildEntity(Engine engine, string key)
    {
        var entity = engine.CreateEntity();

        if (!Entities.TryGetValue(key, out var entityData))
        {
            throw new ArgumentException($"No such entity: {key} found!");
        }

        // Fill out all the normal components
        AppendGeneralComponents(entity, entityData);

        AppendSpecializedComponents(entity, entityData);

        return entity;
    }

    private static void AppendGeneralComponents(Entity entity, EntityFiles entityData)
    {
        if (entityData.EntityConfig is null)
            return;

        var entityConfig = Assets.GetEntityConfig(entityData.EntityConfig);
        foreach ((var type, var component) in entityConfig.Components)
        {
            // TODO: This is gross and slow
            // There's a way to do this where we only pay the reflection cost
            // once per component type
            dynamic castComponent = Convert.ChangeType(component, type);
            entity.AddComponent(castComponent);
        }
    }

    private static void AppendSpecializedComponents(Entity entity, EntityFiles entityData)
    {
        // TODO: Probably split this component and move it to another config
        if (entityData.SpriteTexture is not null)
            entity.AddComponent(new SpriteState
            {
                Texture = Assets.GetTexture(entityData.SpriteTexture),
                Origin = Vector2.Zero,
                Color = Color.White
            });

        if (entityData.SpriteAtlas is not null)
            entity.AddComponent(Assets.GetSpriteAtlas(entityData.SpriteAtlas));

        if (entityData.InputConfig is not null)
            entity.AddComponent(Assets.GetInputConfig(entityData.InputConfig));
    }
}