namespace ECS.Core;

/*
    The idea of placing components inside a native array in a shared pool comes from
    https://github.com/genaray/Arch. This improves performance by maximizing cache
    hits when iterating over components of the same type. It also simplifies the 
    syntax for passing components arround as references to structures. This 
    allows us to use the simple, low-boilerplate definition and instantiation of 
    structs for components, rather than having to define them as classes. We can
    also avoid boxing and unboxing the components themselves, and just downcast
    the pool during access.
*/


// TODO: Remove me if not going to be used
public class ArrayPool<T> where T : struct  // Enforce holding components (struct types)
{
    // TODO: make this resizing to prevent overflows
    private int ArrayCapacity = 512;
    private T[] Array;
    private readonly Dictionary<Entity, int> EntityToIndex = [];
    private readonly Stack<int> ClearedIndices = new();

    public ArrayPool()
    {
        Array = new T[ArrayCapacity];
    }

    // Can overwrite components rather than having a pure add method
    public void SetComponent(Entity entity, in T component)
    {
        if (!EntityToIndex.TryGetValue(entity, out var idx))
        {
            idx = ClearedIndices.TryPop(out int result) ? result : EntityToIndex.Count;
            EntityToIndex[entity] = idx;
        }

        Array[idx] = component;
    }

    public bool RemoveComponent(Entity entity)
    {
        if (!EntityToIndex.Remove(entity, out var idx))
            return false;

        ClearedIndices.Push(idx);
        return true;
    }

    public bool HasComponent(Entity entity)
    {
        return EntityToIndex.ContainsKey(entity);
    }

    // Assumes the entity has the component
    public ref T GetComponent(Entity entity)
    {
        return ref Array[EntityToIndex[entity]];
    }
}