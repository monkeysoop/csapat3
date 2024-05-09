namespace Mekkdonalds.Simulation;

public sealed class Package(Point p)
{
    private static int IDCounter = 1;

    /// <summary>
    /// id of the package
    /// </summary>
    public int ID { get; } = IDCounter++;

    /// <summary>
    /// Position of the package
    /// </summary>
    public Point Position { get; } = p;

    /// <summary>
    /// package constructor
    /// </summary>
    /// <param name="x">x coordinate of the package</param>  
    /// <param name="y">y coordinate of the package</param> 
    public Package(int x, int y) : this(new Point(x, y)) { }

    /// <summary>
    /// resets the ID counter to 1
    /// </summary>
    public static void ResetIDCounter()
    {
        IDCounter = 1;
    }
}
