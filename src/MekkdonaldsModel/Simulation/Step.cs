namespace Mekkdonalds.Simulation;

public readonly struct Step(Point position, int direction, int heuristic)
{
    public Point Position { get; init; } = position;
    public int Direction { get; init; } = direction;
    public int Heuristic { get; init; } = heuristic;
}
