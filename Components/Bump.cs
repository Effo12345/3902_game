namespace ECS.Components;

public class BumpState
{
    public float InitY;
    public float PeakY;
    public float CurrentY;
    public float Speed;
    public float UpSpeed;
    public float DownSpeed;
}

public class Bump
{
    public float Height = 15f;
    public float UpSpeed = 150f;
    public float DownSpeed = 90f;
}