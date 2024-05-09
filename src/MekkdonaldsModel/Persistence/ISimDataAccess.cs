namespace Mekkdonalds.Persistence;

public interface ISimDataAccess
{
    /// <summary>
    /// data access for the config
    /// </summary>
    public IConfigDataAccess CDA { get; init; }
    /// <summary>
    /// data access for the board
    /// </summary>
    public IBoardDataAccess BDA { get; init; }
    /// <summary>
    /// data access for the packages
    /// </summary>
    public IPackagesDataAccess PDA { get; init; }
    /// <summary>
    /// data access for the robots
    /// </summary>
    public IRobotsDataAccess RDA { get; init; }
    /// <summary>
    /// data access for the log files
    /// </summary>
    public ILogFileDataAccess LDA { get; init; }
}