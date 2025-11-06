namespace ECS.Components;

public class BasicEnemy
{
    public String Name;
    public DeathType DeathType;
    public float CleanupTimer;
}

public enum DeathType
{
    Squished,
    Flipped,
    Alive
}