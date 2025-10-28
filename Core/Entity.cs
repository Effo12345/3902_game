namespace ECS.Core;

public class Entity(int id)
{
    public int Id { get; } = id;
    private Dictionary<Type, object> Components = new();

    // Forcibly override component if a same type was already present
    public bool AddComponent<T>(T component) where T : class
    {
        Components[typeof(T)] = component;
        return true;
    }

    public bool RemoveComponent<T>() where T : class
    {
        return Components.Remove(typeof(T));
    }

    // Favor tryget since it is more performant than a hascomponent + getcomponent
    public bool TryGetComponent<T>(out T component) where T : class
    {
        bool valid = Components.TryGetValue(typeof(T), out var componentObj);
        component = null;

        if (valid)
            component = (T)componentObj;

        return valid;
    }

    public bool HasComponent<T>() where T : class
    {
        return Components.ContainsKey(typeof(T));
    }

    // Unsafe get if we know it has it already
    public T GetComponent<T>() where T : class
    {
        return (T)Components[typeof(T)];
    }

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