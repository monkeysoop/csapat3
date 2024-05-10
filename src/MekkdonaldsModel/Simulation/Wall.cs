namespace Mekkdonalds.Simulation;

/// <summary>
/// Wall in the simulation
/// </summary>
/// <param name="x">Coordinate of the wall</param> 
public sealed class Wall(Point x)
{
    /// <summary>
    /// Position of the wall
    /// </summary>
    public Point Position { get; } = x;
    /// <summary>
    /// Constructor for the wall
    /// </summary>
    /// <param name="x">X coordinate of the wall</param> 
    /// <param name="y">Y coordinate of the wall</param> 
    public Wall(int x, int y) : this(new(x, y)) { }
}
