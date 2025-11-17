namespace ECS.Util;

public static class CONSTANTS
{
    public static class LEVEL
    {
        public static string MARIO1_1 { get; } = "Mario1_1";
        public static string TEST_LEVEL { get; } = "TestLevel";
    }
    public static class FILE
    {
        public static string ASSETS { get; } = "Config/assets.json";
    }
    public static class ACTION
    {
        public static string QUIT { get; } = "Quit";
        public static string RESET { get; } = "Reset";
        public static string TEST_LEVEL { get; } = "TestLevel";
        public static string MARIO_LEVEL { get; } = "MarioLevel";
        public static string VELOCITY { get; } = "Velocity";
        public static string ACCELERATION { get; } = "Acceleration";
        public static string LEFT { get; } = "Left";
        public static string RIGHT { get; } = "Right";
        public static string MIDDLE { get; } = "Middle";
        public static string X1 { get; } = "X1";
        public static string X2 { get; } = "X2";
        public static string FIRE { get; } = "Fire";
        public static string JUMP { get; } = "Jump";
    }
    public static class ANIMATION_STATE
    {
        public static string USED_BLOCK { get; } = "used_block";
        public static string SHELL { get; } = "shell";
        public static string KOOPA { get; } = "koopa";
        public static string PLAYER_JUMP_SUFFIX { get; } = "_jump";
        public static string PLAYER_IDLE_SUFFIX { get; } = "_idle";
        public static string PLAYER_WALK_SUFFIX { get; } = "_walk";
    }
    public static class COLLISION
    {
        public static Rectangle QUADTREE_DIMENSION { get; } = new Rectangle(-200,0,1800,480);
        public static float ENEMY_BOUNCE_VELOCITY { get; } = -500;
        public static float ENEMY_FLIP_VELOCITY { get; } = -200;
        public static float SHELL_KICK_SPEED { get; } = 400;
        public static float STAR_JUMP_VELOCITY { get; } = -500;
        public static Vector2 ITEM_VELOCITY { get; } = new Vector2(100, 0);
        public static float ITEM_SPAWN_VELOCITY { get; } = -500;
        public static float ITEM_MASS { get; } = 5;
        public static float ITEM_RISE_SPEED { get; } = 50.0f;
        public static float COIN_RISE_SPEED { get; } = 200.0f;
        public static float MARIO_DEATH_VELOCITY { get; } = -500.0f;
    }
    public static class SOUND
    {
        public static string STOMP { get; } = "Stomp";
        public static string SHELL_KICK { get; } = "ShellKick";
        public static string POWERUP { get; } = "Powerup";
        public static string ONEUP { get; } = "1Up";
        public static string DAMAGE { get; } = "Damage";
        public static string COIN { get; } = "Coin";
        public static string POWERUP_SPAWN { get; } = "PowerupSpawn";
        public static string DEATH { get; } = "Death";
        public static string JUMP { get; } = "MarioJump";
    }
    public static class MATH
    {
        public static float FLOAT_EPSILON { get; } = 0.001f;
    }
    public static class PLAYER_STATE
    {
        public static string DEAD { get; } = "dead";
        public static string SMALL { get; } = "small";
        public static string BIG { get; } = "big";
        public static string FIRE { get; } = "fire";
    }
    public static class TEMP
    {
        public static float DEATH_PLANE { get; } = 500;
        public static float SHELL_TIMER { get; } = 6;
        public static float KOOPA_RESPAWN_HEIGHT { get; } = 34;
        public static float RESPAWN_TIMER { get; } = 3.5f;
        public static float STAR_TIMER { get; } = 2.0f;
        public static float JUMP_VELOCITY { get; } = -300.0f;
    }
    public static class SCORE
    {
        public static int COIN_VALUE {get;} = 200;
        public static int COIN_COUNT {get;} = 1;
        public static int ENEMY_VALUE {get;} = 100;
    }
}