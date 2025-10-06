using ECS.Core;
using System.Collections;
using System.Linq;

namespace ECS.Systems;

public class ItemChangeSystem : AbstractSystem
{
    private List<Entity> Items;
    private int CurrentItemIndex = 0;

    private Keys[] _previousKeys = Array.Empty<Keys>();

    public ItemChangeSystem(List<Entity> items)
    {
        Items = items;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

        foreach (var key in pressedKeys)
        {
            if (!_previousKeys.Contains(key))
            {
                if (key == Keys.Q)
                {
                    if (!Items[CurrentItemIndex].TryGetComponent<Visible>(out var previsible))
                        continue;
                    previsible.Value = false;

                    CurrentItemIndex -= 1;
                    if (CurrentItemIndex < 0)
                        CurrentItemIndex += Items.Count;
                }
                else if (key == Keys.E)
                {
                    if (!Items[CurrentItemIndex].TryGetComponent<Visible>(out var previsible))
                        continue;
                    previsible.Value = false;

                    CurrentItemIndex += 1;
                    if (CurrentItemIndex >= Items.Count)
                        CurrentItemIndex -= Items.Count;
                }

                if (!Items[CurrentItemIndex].TryGetComponent<Visible>(out var curvisible))
                    continue;
                curvisible.Value = true;
            }
        }

        _previousKeys = pressedKeys;
    }
}