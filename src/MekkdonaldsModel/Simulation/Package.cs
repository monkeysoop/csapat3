namespace Mekkdonalds.Simulation;
/// <summary>
/// Package in the simulation
/// </summary>
/// <param name="p">The position of the package</param> 
public sealed class Package(Point p)
{
    private static int IDCounter = 1;

    /// <summary>
    /// Id of the package
    /// </summary>
    public int ID { get; } = IDCounter++;

    /// <summary>
    /// Position of the package
    /// </summary>
    public Point Position { get; } = p;

    /// <summary>
    /// Package constructor
    /// </summary>
    /// <param name="x">X coordinate of the package</param>  
    /// <param name="y">Y coordinate of the package</param> 
    public Package(int x, int y) : this(new Point(x, y)) { }

    /// <summary>
    /// Resets the ID counter to 1
    /// </summary>
    public static void ResetIDCounter()
    {
        IDCounter = 1;
    }
}
