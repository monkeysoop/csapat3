namespace Mekkdonalds.Simulation;

internal readonly struct Step
{
    public Point Position { get; init; }
    public int Direction { get; init; }
    public int Heuristic { get; init; }

    public Step(Point position, int direction, int heuristic)
    {
        Position = position;
        Direction = direction;
        Heuristic = heuristic;
    }
}
