
namespace ECS.Systems;

public class CameraSystem : AbstractSystem
{
    private const float SCROLL_WIDTH = 0.4f;

    private readonly GraphicsManager Graphics;

    private float _midX; // determines where one wants to regard as middle of the screen
    private Entity _center;

    public CameraSystem(GraphicsManager graphics)
    {
        Graphics = graphics;

        _center = null;
        _midX = graphics.SpriteBatch.GraphicsDevice.Viewport.Width * SCROLL_WIDTH;
    }

    private static bool IsMario(Entity entity)
    {
        return entity.TryGetComponent<Position>(out var _)
            && entity.TryGetComponent<InputBindings>(out var _);
    }

    // TODO: Move camera adjustments to their own system
    private Vector2 PositionAdjustment()
    {
        foreach (var entity in Engine.GetEntities())
        {
            if (IsMario(entity))
            {
                _center = entity;
                break;
            }
        }

        if (_center == null)
        {
            // If the following loop finds nothing, use (0, 0)
            return new Vector2(_midX, CONSTANTS.RENDER.VERTICAL_CAMERA_OFFSET);
        }
        else
        {
            Position position;
            _center.TryGetComponent<Position>(out position);
            // Currently, Y position is designed to be not adjusted
            return new Vector2(_midX - position.Value.X, CONSTANTS.RENDER.VERTICAL_CAMERA_OFFSET);
        }
    }


    public override void Update(Engine engine, GameTime gameTime)
    {
        // Get the transformation matrix needed to offset by the camera amount...
        Vector2 cameraOffset = PositionAdjustment();
        Graphics.CameraTransform = Matrix.CreateTranslation(cameraOffset.X, cameraOffset.Y, 0);
    }
}