using System.Diagnostics.CodeAnalysis;

namespace Mekkdonalds.Simulation.Assigner;

/// <summary>
/// Handles the logic behind which packages to assign to which robots.
/// </summary>
/// <param name="board">Map of the warehouse</param>
/// <param name="packages">Collection of the packages that will be assigned</param>
/// <param name="robots">Collection of the robots that are part of the simulation</param>
public abstract class Assigner(Board board, IEnumerable<Package> packages, IEnumerable<Robot> robots)
{
    protected readonly IEnumerable<Package> _packages = packages;
    protected readonly IEnumerable<Robot> _robots = robots;
    protected Board _board = board;

    /// <summary>
    /// <see langword="true"/> if there are no packages left in the assigner.
    /// </summary>
    public virtual bool NoPackage { get; }

    /// <summary>
    /// Peeks at the next package for the robot.
    /// </summary>
    /// <param name="robot">The robot whose next package to peek at</param>
    /// <param name="package">Contains the package</param>
    /// <returns>Returns <see langword="true"/> of the Peek was successful</returns>
    public abstract bool Peek(Robot robot, [MaybeNullWhen(false)] out Package package);
    /// <summary>
    /// Gets the next package for the robot. (Does not call the robot's Assign method)
    /// </summary>
    /// <param name="robot">The robot whose package to get</param>
    /// <param name="package">Contains the package</param>
    /// <returns>Returns <see langword="true"/> of the getting the package was successful</returns>
    public abstract bool Get(Robot robot, [MaybeNullWhen(false)] out Package package);
    /// <summary>
    /// Returns an uncompleted package to the assigner.
    /// </summary>
    /// <param name="package">The uncompleted package</param>
    public virtual void Return(Package package) { }
}
