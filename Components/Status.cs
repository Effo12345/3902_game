namespace ECS.Components;

public class Status
{
    public string State; //Can be "small" "big" "fire" or "dead"
    public float InvincibilityDuration;
    public bool isInvincible;
    public bool isStar;
    public int Lives;
    public int Coins;

    public int NumFireballs = 0;
    // public int Health;
    // public int Score;
}