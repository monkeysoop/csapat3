namespace Mekkdonalds.Simulation;

public sealed class Wall(Point x)
{
    /// <summary>
    /// position of the wall
    /// </summary>
    public Point Position { get; } = x;
    /// <summary>
    /// constructor for the wall
    /// </summary>
    /// <param name="x">x coordinate of the wall</param> 
    /// <param name="y">y coordinate of the wall</param> 
    public Wall(int x, int y) : this(new(x, y)) { }
}
