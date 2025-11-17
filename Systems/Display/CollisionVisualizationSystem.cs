namespace ECS.Systems;

public class CollisionVisualizationSystem : AbstractSystem
{
    private const int LINE_WEIGHT = 2;
    private readonly Dictionary<Direction, Direction> DirInverter;

    private readonly GraphicsDevice Device;
    private readonly SpriteBatch SpriteBatch;
    private readonly Texture2D LineTexture;

    private Matrix LatestCameraTransform;
    private Dictionary<Entity, HashSet<Direction>> Collisions = [];

    public CollisionVisualizationSystem(GraphicsDevice graphics, SpriteBatch spriteBatch)
    {
        Device = graphics;
        SpriteBatch = spriteBatch;

        LineTexture = new(Device, 1, 1);
        LineTexture.SetData([Color.White]);

        DirInverter = new()
        {
            { Direction.Top, Direction.Bottom },
            { Direction.Bottom, Direction.Top },
            { Direction.Left, Direction.Right },
            { Direction.Right, Direction.Left }
        };
    }

    public override bool Initialize(Engine engine)
    {
        base.Initialize(engine);

        engine.PubSub.CreateSubscription<CameraTransformEvent>(OnCameraTransform);
        engine.PubSub.CreateSubscription<CollisionEvent>(OnCollision);

        return true;
    }

    public override void Update(Engine engine, GameTime gameTime)
    {
        SpriteBatch.Begin(transformMatrix: LatestCameraTransform);

        foreach (var entity in engine.GetEntities())
        {
            // TODO: Only do this in debug mode
            if (!entity.TryGetComponent<Collider>(out var collider))
                continue;

            Rectangle box = collider.HitBox;
            int x = box.X, y = box.Y, w = box.Width, h = box.Height;

            var boxLines = new[]
            {
                (new Rectangle(x, y, w, LINE_WEIGHT), Direction.Top),
                (new Rectangle(x + w, y, LINE_WEIGHT, h), Direction.Right),
                (new Rectangle(x, y + h, w, LINE_WEIGHT), Direction.Bottom),
                (new Rectangle(x, y, LINE_WEIGHT, h), Direction.Left)
            };

            foreach (var line in boxLines)
            {
                // Red if there was a collision that side this update, black otherwise
                Color color = (Collisions.TryGetValue(entity, out var set) && set.Contains(line.Item2)) ? Color.Red : Color.Black;
                SpriteBatch.Draw(
                    LineTexture,
                    line.Item1,
                    color
                );
            }
        }

        SpriteBatch.End();
        Collisions = [];
    }

    private void OnCameraTransform(IEvent evt)
    {
        LatestCameraTransform = ((CameraTransformEvent)evt).Transform;
    }

    private void OnCollision(IEvent evt)
    {
        var collisionEvent = (CollisionEvent)evt;

        // Make a new set if one isn't found, then add the direction
        void updateColliders(Entity entity, Direction direction)
        {
            if (!Collisions.TryGetValue(entity, out var set))
                Collisions[entity] = set = [];
            set.Add(direction);
        }

        updateColliders(collisionEvent.MovingEntity, collisionEvent.Direction);
        updateColliders(collisionEvent.SecondaryEntity, DirInverter[collisionEvent.Direction]);
    }
}