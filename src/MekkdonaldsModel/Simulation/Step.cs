namespace Mekkdonalds.Simulation;

public readonly struct Step(Point position, int direction, int heuristic)
{
    /// <summary>
    /// position of the robot
    /// </summary>
    public Point Position { get; init; } = position;
    /// <summary>
    /// direction the robot is facing
    /// </summary>
    public int Direction { get; init; } = direction;
    /// <summary>
    /// heuristic value of the step
    /// </summary>
    public int Heuristic { get; init; } = heuristic;
}
