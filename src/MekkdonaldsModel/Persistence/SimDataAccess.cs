namespace Mekkdonalds.Persistence;

public class SimDataAccess : ISimDataAccess
{
    public required IConfigDataAccess CDA { get; init; }
    public required IBoardDataAccess BDA { get; init; }
    public required IPackagesDataAccess PDA { get; init; }
    public required IRobotsDataAccess RDA { get; init; }
}