namespace Mekkdonalds.Persistence;

public interface ISimDataAccess
{
    public IConfigDataAccess CDA { get; init; }
    public IBoardDataAccess BDA { get; init; }
    public IPackagesDataAccess PDA { get; init; }
    public IRobotsDataAccess RDA { get; init; }
    public ILogFileDataAccess LDA { get; init; }
}