namespace Mekkdonalds.Simulation;

public sealed class Package(Point p)
{
    private static int IDCounter = 1;

    public int ID { get; } = IDCounter++;

    public Point Position { get; } = p;

    public Package(int x, int y) : this(new Point(x, y)) { }
}
