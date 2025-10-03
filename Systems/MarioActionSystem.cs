using ECS.Core;
using System.Collections;
using System.Linq;
using System.Security.Cryptography;

namespace ECS.Systems;

public class MarioActionSystem : AbstractSystem
{
    private Entity Mario;
    private Keys[] _previousKeys = Array.Empty<Keys>();

    public MarioActionSystem(Entity mario)
    {
        Mario = mario;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

        foreach (var key in pressedKeys)
        {
            if (!_previousKeys.Contains(key))
            {
                if (!Mario.TryGetComponent<Status>(out var currentStatus))
                    continue;

                if (key == Keys.F)
                    currentStatus.State = "small";
                else if (key == Keys.G)
                    currentStatus.State = "big";
                else if (key == Keys.H)
                    currentStatus.State = "fire";
                
            }
        }

        _previousKeys = pressedKeys;
    }
}