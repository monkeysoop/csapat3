namespace Mekkdonalds.Simulation;

/// <summary>
/// Package in the simulation
/// </summary> 
public sealed class Package
{
    private static int IDCounter = 1;

    /// <summary>
    /// Id of the package
    /// </summary>
    public int ID { get; } = IDCounter++;

    /// <summary>
    /// Position of the package
    /// </summary>
    public Point Position { get; }

    /// <summary>
    /// Creates a new package
    /// </summary>
    /// <param name="p">The position of the package</param> 
    public Package(Point p)
    {
        Position = p;
    }

    /// <summary>
    /// Package constructor
    /// </summary>
    /// <param name="x">X coordinate of the package</param>  
    /// <param name="y">Y coordinate of the package</param> 
    public Package(int x, int y) : this(new Point(x, y)) { }

    /// <summary>
    /// Resets the ID counter to 1
    /// </summary>
    internal static void ResetIDCounter()
    {
        IDCounter = 1;
    }
}
