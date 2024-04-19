namespace Mekkdonalds.Simulation;

public sealed class Package(Point p) : IMapObject
{
    private static int IDCounter = 1;

    public readonly int ID = IDCounter++;

    public Point Position { get; } = p;

    public Package(int x, int y) : this(new Point(x, y)) { }
}
