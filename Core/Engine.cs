using System.Collections.Generic;

namespace ECS.Core;

// Inspiration and reference for the ECS system comes from https://www.richardlord.net/blog/ecs/what-is-an-entity-framework
public class Engine
{
    // Fast to search for entity in hashset by ID
    private readonly HashSet<Entity> Entities = [];
    private int NextEntityID = 0;

    private List<ISystem> Systems;

    // Force instantiation to happen inside the engine
    public Entity CreateEntity()
    {
        Entity entity = new(NextEntityID);
        NextEntityID++;
        Entities.Add(entity);

        return entity;
    }

    public bool DestroyEntity(Entity entity)
    {
        return Entities.Remove(entity);
    }

    public HashSet<Entity> GetEntities()
    {
        return Entities;
    }

    public void AddSystem(ISystem system)
    {
        Systems.Add(system);
    }

    public void RemoveSystem(ISystem system)
    {
        Systems.Remove(system);
    }

    public void Update(GameTime gameTime)
    {
        foreach (var system in Systems)
        {
            system.Update(this, gameTime);
        }
    }
}