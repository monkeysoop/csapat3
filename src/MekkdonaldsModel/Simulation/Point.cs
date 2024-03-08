namespace Mekkdonalds.Simulation;

public class Point(int x, int y)
{
    public readonly int X = x;
    public readonly int Y = y;

    public Point() : this(0, 0) { }
}
