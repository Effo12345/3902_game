namespace ECS.Systems;

public class MarioActionSystem : AbstractSystem
{
    private Entity Mario;
    private Texture2D Fireballsprite;

    private Keys[] _previousKeys = Array.Empty<Keys>();

    public MarioActionSystem(Entity mario, Texture2D fireballsprite)
    {
        Mario = mario;
        Fireballsprite = fireballsprite;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

        foreach (var key in pressedKeys)
        {
            if (!_previousKeys.Contains(key))
            {
                if (!Mario.TryGetComponent<Status>(out var currentStatus) ||
                    !Mario.TryGetComponent<Position>(out var position) ||
                    !Mario.TryGetComponent<FacingDirection>(out var facingDirection)
                    )
                    continue;

                if (key == Keys.F)
                    currentStatus.State = "small";
                else if (key == Keys.G)
                    currentStatus.State = "big";
                else if (key == Keys.H)
                    currentStatus.State = "fire";
                else if (key == Keys.L)
                    if (currentStatus.State == "fire")
                    {
                        Vector2 direction;
                        if (facingDirection.IsFacingLeft)
                            direction = new Vector2(-1.0f, 0.0f);
                        else
                            direction = new Vector2(1.0f, 0.0f);

                        var fireball = Engine.CreateEntity();
                        var itemConfig = SpriteSheetLoader.Load(File.ReadAllText("Content/Config/items_spritesheet.json"));

                        // TODO: Fix the spawn position for fireball
                        fireball.AddComponent(new Position { Value = position.Value + new Vector2(20.0f, 50.0f) });
                        fireball.AddComponent(new Rotation { Value = 0f });
                        fireball.AddComponent(new Scale { Value = new Vector2(1.5f, 1.5f) });
                        fireball.AddComponent(new Velocity { Value = direction * 250f });
                        fireball.AddComponent(new VelocityCap());
                        fireball.AddComponent(new Acceleration());
                        fireball.AddComponent(new Mass { Value = 2.5f });
                        fireball.AddComponent(new SpriteState { Texture = Fireballsprite, SourceRect = itemConfig.States["Fireball"].Frames[0], Color = Color.White });
                        fireball.AddComponent(new AnimationConfig { States = itemConfig.States });
                        fireball.AddComponent(new AnimationState { CurrentState = "Fireball" });
                    }
            }
        }

        _previousKeys = pressedKeys;
    }
}