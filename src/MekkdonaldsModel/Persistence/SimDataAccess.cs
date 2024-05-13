namespace Mekkdonalds.Persistence;
/// <summary>
/// Data access object for the simulation
/// </summary>
public class SimDataAccess : ISimDataAccess
{
    /// <summary>
    /// Data access object for the configuration
    /// </summary>
    public required IConfigDataAccess ConfigDataAccess { get; init; }
    /// <summary>
    /// Data access object for the board
    /// </summary>
    public required IBoardDataAccess BoardDataAccess { get; init; }
    /// <summary>
    /// Data access object for the packages
    /// </summary>
    public required IPackagesDataAccess PackagesDataAccess { get; init; }
    /// <summary>
    /// Data access object for the robots
    /// </summary>
    public required IRobotsDataAccess RobotsDataAccess { get; init; }
    /// <summary>
    /// Data access object for the log files
    /// </summary>
    public required ILogFileDataAccess LogFileDataAccess { get; init; }

    private static SimDataAccess? _instance;

    /// <summary>
    /// Creates a new instance of the SimDataAccess class
    /// </summary>
    public static SimDataAccess Instance => _instance ??= new()
    {
        ConfigDataAccess = new ConfigDataAccess(),
        BoardDataAccess = new BoardFileDataAccess(),
        RobotsDataAccess = new RobotsDataAccess(),
        PackagesDataAccess = new PackagesDataAccess(),
        LogFileDataAccess = new LogFileDataAccess()
    };
}
