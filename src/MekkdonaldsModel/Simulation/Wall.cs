namespace Mekkdonalds.Simulation;

public class Wall(Point x) : IMapObject
{
    public Point Position { get; } = x;

    public Wall(int x, int y) : this(new(x, y)) { }
}
