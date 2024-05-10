namespace Mekkdonalds.Persistence;

/// <summary>
/// Data access interface for the simulation
/// </summary>
public interface ISimDataAccess
{
    /// <summary>
    /// Data access for the config
    /// </summary>
    public IConfigDataAccess CDA { get; init; }
    /// <summary>
    /// Data access for the board
    /// </summary>
    public IBoardDataAccess BDA { get; init; }
    /// <summary>
    /// Data access for the packages
    /// </summary>
    public IPackagesDataAccess PDA { get; init; }
    /// <summary>
    /// Data access for the robots
    /// </summary>
    public IRobotsDataAccess RDA { get; init; }
    /// <summary>
    /// Data access for the log files
    /// </summary>
    public ILogFileDataAccess LDA { get; init; }
}