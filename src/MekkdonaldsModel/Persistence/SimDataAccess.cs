namespace Mekkdonalds.Persistence;

public class SimDataAccess : ISimDataAccess
{
    public required IConfigDataAccess CDA { get; init; }
    public required IBoardDataAccess BDA { get; init; }
    public required IPackagesDataAccess PDA { get; init; }
    public required IRobotsDataAccess RDA { get; init; }
    public required ILogFileDataAccess LDA { get; init; }

    private static SimDataAccess? _instance;

    public static SimDataAccess Instance => _instance ??= new()
    {
        CDA = new ConfigDataAccess(),
        BDA = new BoardFileDataAccess(),
        RDA = new RobotsDataAccess(),
        PDA = new PackagesDataAccess(),
        LDA = new LogFileDataAccess()
    };
}