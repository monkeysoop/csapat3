namespace Mekkdonalds.Persistence;

/// <summary>
/// Interface that contains the data access classes for the simulation
/// </summary>
public interface ISimDataAccess
{
    public IConfigDataAccess ConfigDataAccess { get; init; }
    public IBoardDataAccess BoardDataAccess { get; init; }
    public IPackagesDataAccess PackagesDataAccess { get; init; }
    public IRobotsDataAccess RobotsDataAccess { get; init; }
    public ILogFileDataAccess LogFileDataAccess { get; init; }
}