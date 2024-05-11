namespace Mekkdonalds.Persistence;

public class SimDataAccess : ISimDataAccess
{
    public required IConfigDataAccess ConfigDataAccess { get; init; }
    public required IBoardDataAccess BoardDataAccess { get; init; }
    public required IPackagesDataAccess PackagesDataAccess { get; init; }
    public required IRobotsDataAccess RobotsDataAccess { get; init; }
    public required ILogFileDataAccess LogFileDataAccess { get; init; }

    private static SimDataAccess? _instance;

    public static SimDataAccess Instance => _instance ??= new()
    {
        ConfigDataAccess = new ConfigDataAccess(),
        BoardDataAccess = new BoardFileDataAccess(),
        RobotsDataAccess = new RobotsDataAccess(),
        PackagesDataAccess = new PackagesDataAccess(),
        LogFileDataAccess = new LogFileDataAccess()
    };
}