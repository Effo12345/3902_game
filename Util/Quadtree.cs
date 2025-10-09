namespace ECS.Util;

public class Quadtree
{
    private readonly List<Entity> entities = new();
    private Rectangle Bounds;
    private Quadtree topLeft;
    private Quadtree topRight;
    private Quadtree bottomLeft;
    private Quadtree bottomRight;

    public Quadtree(Rectangle bounds)
    {
        Bounds = bounds;
    }

}