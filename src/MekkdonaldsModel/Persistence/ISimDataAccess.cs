namespace Mekkdonalds.Persistence;

/// <summary>
/// Interface that contains the data access classes for the simulation
/// </summary>
public interface ISimDataAccess
{
    /// <summary>
    /// Data access for the config
    /// </summary>
    public IConfigDataAccess ConfigDataAccess { get; init; }
    /// <summary>
    /// Data access for the board
    /// </summary>
    public IBoardDataAccess BoardDataAccess { get; init; }
    /// <summary>
    /// Data access for the packages
    /// </summary>
    public IPackagesDataAccess PackagesDataAccess { get; init; }
    /// <summary>
    /// Data access for the robots
    /// </summary>
    public IRobotsDataAccess RobotsDataAccess { get; init; }
    /// <summary>
    /// Data access for the log files
    /// </summary>
    public ILogFileDataAccess LogFileDataAccess { get; init; }
}
