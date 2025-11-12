// CURRENTLY ARCHIVED
// MOSTLY FUNCTIONAL BUT NEEDS TWEAKS FOR THE NEW SYSTEMS

namespace ECS.Systems;

public class DebrisSpawnSystem : AbstractSystem
{
    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);
        Engine.PubSub.CreateSubscription<DebrisSpawn>(DrawDebris);
        return true;
    }

    private void DrawDebris(IEvent evnt)
    {
        var e = (DebrisSpawn)evnt;
        var src = e.Sprite.SourceRect;

        //compute half sizes
        int halfWidth = src.Width / 2;
        int halfHeight = src.Height / 2;

        //divide into four sub rectangles
        var topLeft = new Rectangle(src.X, src.Y, halfWidth, halfHeight);
        var topRight = new Rectangle(src.X + halfWidth, src.Y, src.Width - halfWidth, halfHeight);
        var bottomLeft = new Rectangle(src.X, src.Y + halfHeight, halfWidth, halfHeight);
        var bottomRight = new Rectangle(src.X + halfWidth, src.Y + halfHeight, src.Width - halfWidth, halfHeight);

        //spawn four fragments each in different directions
        SpawnFrag(e.Position, e.Sprite, topLeft, new Vector2(-1.0f, -1.4f), e.Break);
        SpawnFrag(e.Position, e.Sprite, topRight, new Vector2(1.0f, -1.5f), e.Break);
        SpawnFrag(e.Position, e.Sprite, bottomLeft, new Vector2(-0.8f, -1.1f), e.Break);
        SpawnFrag(e.Position, e.Sprite, bottomRight, new Vector2(0.9f, -1.2f), e.Break);
    }
    private void SpawnFrag(Position pos, SpriteState s, Rectangle rect, Vector2 direction, Break br)
    {
        //create frag entity
        var frag = Engine.CreateEntity();
        //add frag components
        frag.AddComponent(new Position { Value = pos.Value });
        frag.AddComponent(new Velocity { Value = direction * br.FragSpeed });
        frag.AddComponent(new Acceleration { Value = new Vector2(0f, br.Acceleration) });
        frag.AddComponent(new Rotation { Value = 0f });
        frag.AddComponent(new Scale { Value = Vector2.One });
        frag.AddComponent(new SpriteState
        {
            Texture = s.Texture,
            SourceRect = rect,
            Origin = new Vector2(rect.Width * 0.5f, rect.Height * 0.5f),
            Color = s.Color
        });
    }
    public override void Update(Engine engine, GameTime time) { }
}