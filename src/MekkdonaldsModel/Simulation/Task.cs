namespace Mekkdonalds.Simulation;

public class Task(Point p) : IMapObject
{
    public Point Position { get; } = p;

    public Task(int x, int y) : this(new Point(x, y)) { }
}
