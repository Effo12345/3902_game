using System;
using System.Collections.Generic;

namespace ECS.Core;

public class Entity(int id)
{
    public int Id { get; } = id;
    private Dictionary<Type, object> Components = new();

    public bool AddComponent<T>(T component) where T : class
    {
        return Components.TryAdd(typeof(T), component);
    }

    public bool RemoveComponent<T>() where T : class
    {
        return Components.Remove(typeof(T));
    }

    public bool TryGetComponent<T>(out T component) where T : class
    {
        bool valid = Components.TryGetValue(typeof(T), out var componentObj);
        component = null;

        if (valid)
            component = (T)componentObj;

        return valid;
    }

    // public T GetComponent<T>() where T : class
    // {
    //     // TODO: Find a better way to handle this gracefully
    //     if (!Components.TryGetValue(typeof(T), out var obj))
    //     {
    //         Console.WriteLine($"Failed getting component on entity {Id}. Returned value is invalid");
    //     }

    //     return (T)obj;
    // }

    // Object overrides to make it fast to search for entities by ID
    public override int GetHashCode()
    {
        return Id;
    }

    public override bool Equals(object obj)
    {
        return obj is Entity entity && entity.Id == Id;
    }
}