namespace ECS.Components;

public class Status
{
    public string State; //Can be "small" "big" "fire" "sunshine" or "dead"
    public float InvincibilityDuration;
    public bool isInvincible;
    public bool isHovering; 
    public bool canHover;
    public float StarTimer;
    public int Lives;
    public int Coins;
    public Direction InputDirection;

    public int NumFireballs = 0;
    // public int Health;
    // public int Score;
}