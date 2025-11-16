using Microsoft.VisualBasic;

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

    public static void AppendComponentsFromConfig(Entity entity, EntityConfig config)
    {
        foreach ((var type, var component) in config.Components)
        {
            // TODO: This is gross and slow
            // There's a way to do this where we only pay the reflection cost
            // once per component type
            dynamic castComponent = Convert.ChangeType(component, type);

            // We need to (shallow) copy the component or all entities
            // of the same type will share a single instance
            entity.AddComponent(Clone(castComponent));
        }
    }

    /**
     * Courtesy of https://stackoverflow.com/a/39666660
     * Uses reflection to conduct a shallow clone of passed objects.
     * Calls the MemberwiseClone method defined in object but which is normally protected.
    */
    private static T Clone<T>(T obj)
    {
        var inst = obj.GetType().GetMethod(
            "MemberwiseClone",
            System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic
        );
        return (T)inst?.Invoke(obj, null);
    }

    private static void AppendGeneralComponents(Entity entity, EntityFiles entityData)
    {
        if (entityData.EntityConfig is null)
            return;

        var entityConfig = Assets.GetEntityConfig(entityData.EntityConfig);
        AppendComponentsFromConfig(entity, entityConfig);
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
    public static void BuildCompositeEntity(Engine engine, Entity parent)
    {
        if(!parent.TryGetComponent<CompositeDefinition>(out var def) || !parent.TryGetComponent<Position>(out var parentPos)) return;

        var composite = new CompositeEntity();
        parent.AddComponent(composite);
        
        foreach(var childDef in def.Children)
        {
            var child = BuildEntity(engine, childDef.Key);
            if(child.TryGetComponent<Position>(out var childPos))
            {
                childPos.Value = parentPos.Value + childDef.Offset;
            }
            if(!string.IsNullOrEmpty(childDef.PartName))
                composite.Parts[childDef.PartName] = child;
        }
    }
}