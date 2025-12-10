using System.Net.NetworkInformation;

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
        public static string CROUCH { get; } = "Crouch";
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
        public static string BIG_PEACE_SIGN { get; } = "big_peace";
    }
    public static class COLLISION
    {
        public static Rectangle QUADTREE_DIMENSION { get; } = new Rectangle(-200,-200,20000,680);
        public static float ENEMY_BOUNCE_VELOCITY { get; } = -500;
        public static float ENEMY_FLIP_VELOCITY { get; } = -200;
        public static float SHELL_KICK_SPEED { get; } = 400;
        public static float ITEM_JUMP_VELOCITY { get; } = -300;
        public static Vector2 ITEM_VELOCITY { get; } = new Vector2(100, 0);
        public static float ITEM_SPAWN_VELOCITY { get; } = -300;
        public static float ITEM_MASS { get; } = 5;
        public static float ITEM_RISE_SPEED { get; } = 50.0f;
        public static float COIN_RISE_SPEED { get; } = 200.0f;
        public static float MARIO_DEATH_VELOCITY { get; } = -1000.0f;
    }
    public static class SOUND
    {
        public static string STOMP { get; } = "Stomp";
        public static string SHELL_KICK { get; } = "ShellKick";
        public static string FIREBALL { get; } = "Fireball";
        public static string POWERUP { get; } = "Powerup";
        public static string ONEUP { get; } = "1Up";
        public static string DAMAGE { get; } = "Damage";
        public static string COIN { get; } = "Coin";
        public static string POWERUP_SPAWN { get; } = "PowerupSpawn";
        public static string DEATH { get; } = "Death";
        public static string JUMP { get; } = "MarioJump";
        public static string COURSE_CLEAR { get; } = "CourseClear";
        public static string BLOCKBREAK { get; } = "BlockBreak";
        public static string STAR { get; } = "Star";
        public static string PIPE_ENTER { get; } = "PipeEnter";
    }
    public static class MATH
    {
        public static float FLOAT_EPSILON { get; } = 0.001f;
        public static byte BYTE_MAX { get; } = 255;
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
        public static Vector2 FIREBALL_SPAWN { get; } = new Vector2(15 * 3 / 2);
        public static float RESPAWN_TIMER { get; } = 3.5f;
        public static float STAR_TIMER { get; } = 8.0f;
        public static float JUMP_VELOCITY { get; } = -500.0f;
        public static float MAX_JUMP_HOLD_FRAMES { get; } = 20;

        public static Color DEFAULT_BACKGROUND_COLOR { get; } = Color.CornflowerBlue;
    }
    public static class RENDER
    {
        public static Vector2 WindowSize { get; } = new Vector2(800, 800);
        public static float VERTICAL_CAMERA_OFFSET { get; } = 320;
    }
    public static class TAG
    {
        public static string PLAYER { get; } = "player";
        public static string FLAG { get; } = "flag";
        public static string FLAG_BAR { get; } = "flag_bar";
    }
    public static class SHADER
    {
        public static string PIXELATION_SHADER { get; } = "PixelationShader";
        public static string PIXEL_SIZE_PROPERTY { get; } = "PixelSize";
    }
    public static class SCORE
    {
        public static int COIN_VALUE {get;} = 200;
        public static int COIN_COUNT {get;} = 1;
        public static int ENEMY_VALUE {get;} = 100;
    }
}