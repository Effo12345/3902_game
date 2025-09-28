namespace ECS.Systems;

public class BlockClickSystem : AbstractSystem
{
    private MouseState _prev;

    public override void Update(Engine engine, GameTime gameTime)
    {
        var mouse   = Mouse.GetState();
        bool clicked = mouse.LeftButton == ButtonState.Pressed &&
                       _prev.LeftButton  == ButtonState.Released;

        if (!clicked)
        {
            _prev = mouse;
            return;
        }

        var point = mouse.Position;

        foreach (var e in engine.GetEntities())
        {
            if (!TryGetHitRect(e, out var rect))
                continue;
            if (!rect.Contains(point))
                continue;

            if (e.TryGetComponent<Bump>(out _))
                Engine.PubSub.Publish(new BlockBump { Entity = e });

            if (e.TryGetComponent<Break>(out _))
                Engine.PubSub.Publish(new BlockBreak { Entity = e });

            break; 
        }

        _prev = mouse;
    }

    private static bool TryGetHitRect(Entity e, out Rectangle rect)
    {
        rect = default;

        if (!e.TryGetComponent<SpriteState>(out var sprite)) 
            return false;
        if (!e.TryGetComponent<Position>(out var pos))
            return false;

        float sx = 1f, sy = 1f;
        if (e.TryGetComponent<Scale>(out var sc))
        {
            sx = sc.Value.X;
            sy = sc.Value.Y;
        }

        var src    = sprite.SourceRect;
        var origin = sprite.Origin;

        var topLeft = new Vector2(
            pos.Value.X - origin.X * sx,
            pos.Value.Y - origin.Y * sy
        );

        rect = new Rectangle(
            (int)topLeft.X,
            (int)topLeft.Y,
            (int)(src.Width  * sx),
            (int)(src.Height * sy)
        );
        return true;
    }
}
