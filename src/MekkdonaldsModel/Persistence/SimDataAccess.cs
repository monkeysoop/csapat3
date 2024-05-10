namespace Mekkdonalds.Persistence;
/// <summary>
/// Data access object for the simulation
/// </summary>
public class SimDataAccess : ISimDataAccess
{

    /// <summary>
    /// Data access object for the configuration
    /// </summary>
    public required IConfigDataAccess CDA { get; init; }
    /// <summary>
    /// Data access object for the board
    /// </summary>
    public required IBoardDataAccess BDA { get; init; }
    /// <summary>
    /// Data access object for the packages
    /// </summary>
    public required IPackagesDataAccess PDA { get; init; }
    /// <summary>
    /// Data access object for the robots
    /// </summary>
    public required IRobotsDataAccess RDA { get; init; }
    /// <summary>
    /// Data access object for the log files
    /// </summary>
    public required ILogFileDataAccess LDA { get; init; }

    private static SimDataAccess? _instance;

    /// <summary>
    /// Creates a new instance of the SimDataAccess class
    /// </summary>
    public static SimDataAccess Instance => _instance ??= new()
    {
        CDA = new ConfigDataAccess(),
        BDA = new BoardFileDataAccess(),
        RDA = new RobotsDataAccess(),
        PDA = new PackagesDataAccess(),
        LDA = new LogFileDataAccess()
    };
}