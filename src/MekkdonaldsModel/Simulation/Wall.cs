namespace Mekkdonalds.Simulation;

public sealed class Wall(Point x)
{
    public Point Position { get; } = x;

    public Wall(int x, int y) : this(new(x, y)) { }
}
