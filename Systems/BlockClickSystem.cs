namespace ECS.Systems;

/// <summary>
/// For sprint 2 only. Used to demonstrate block animations.
/// Will be moved to a specialized mouse system later
/// </summary>
public class BlockClickSystem : AbstractSystem
{
    private MouseState _prev;

    public override void Update(Engine engine, GameTime gameTime)
    {
        var mouse = Mouse.GetState();
        //Is new click
        bool clicked = mouse.LeftButton == ButtonState.Pressed &&
                       _prev.LeftButton == ButtonState.Released;
        //Update if no new click
        if (!clicked)
        {
            _prev = mouse;
            return;
        }
        //Get mouse position
        var point = mouse.Position;
        //Loop through entities
        foreach (var e in engine.GetEntities())
        {
            //Skip if entity do not have area rectangle
            if (!TryGetHitRect(e, out var rect))
                continue;
            //Skip if click is not in entity's area rectangle
            if (!rect.Contains(point))
                continue;
            //If entity have Bump component, trigger bump behavior
            if (e.TryGetComponent<Bump>(out _))
                Engine.PubSub.Publish(new BlockBump { Entity = e });
                //If entity have Break component, trigger break behavior
            if (e.TryGetComponent<Break>(out _))
                Engine.PubSub.Publish(new BlockBreak { Entity = e });
            break;
        }
        _prev = mouse;
    }

    //Attempts to compute the clickable rectangle area for an entity
    private static bool TryGetHitRect(Entity e, out Rectangle rect)
    {
        rect = default;
        //Check if entity have sprite and position components
        if (!e.TryGetComponent<SpriteState>(out var sprite))
            return false;
        if (!e.TryGetComponent<Position>(out var pos))
            return false;

        //Default scale 1
        float sx = 1f, sy = 1f;
        //Overwrite scale
        if (e.TryGetComponent<Scale>(out var sc))
        {
            sx = sc.Value.X;
            sy = sc.Value.Y;
        }
        //Size of the sprite
        var src = sprite.SourceRect;
        //Origin point of sprite
        var origin = sprite.Origin;
        //Calculate top left position of sprite
        var topLeft = new Vector2(
            pos.Value.X - origin.X * sx,
            pos.Value.Y - origin.Y * sy
        );
        //Build rectangle
        rect = new Rectangle(
            (int)topLeft.X,
            (int)topLeft.Y,
            (int)(src.Width * sx),
            (int)(src.Height * sy)
        );
        return true;
    }
}
