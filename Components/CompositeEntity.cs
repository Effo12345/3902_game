namespace ECS.Components;

public class ChildEntityDefinition
{
    public string Key;
    public string PartName;
    public Vector2 Offset;
}

public class CompositeDefinition
{
    public ChildEntityDefinition[] Children;
}

public class CompositeEntity
{
    public Dictionary<string, Entity> Parts {get;} = new();
}