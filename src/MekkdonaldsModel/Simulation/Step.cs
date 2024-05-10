namespace Mekkdonalds.Simulation;

/// <summary>
/// Step in the simulation
/// </summary>
/// <param name="position"> The position of the robot</param>
/// <param name="direction"> The direction the robot is facing</param>
/// <param name="heuristic">The heuristic value of the step</param> 
public readonly struct Step(Point position, int direction, int heuristic)
{
    /// <summary>
    /// Position of the robot
    /// </summary>
    public Point Position { get; init; } = position;
    /// <summary>
    /// Direction the robot is facing
    /// </summary>
    public int Direction { get; init; } = direction;
    /// <summary>
    /// Heuristic value of the step
    /// </summary>
    public int Heuristic { get; init; } = heuristic;
}
