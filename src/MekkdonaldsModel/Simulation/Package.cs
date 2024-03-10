namespace Mekkdonalds.Simulation;

public class Package(Point p) : IMapObject
{
    public Point Position { get; } = p;

    public Package(int x, int y) : this(new Point(x, y)) { }
}
