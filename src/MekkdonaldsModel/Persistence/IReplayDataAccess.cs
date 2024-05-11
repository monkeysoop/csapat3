namespace Mekkdonalds.Persistence;

/// <summary>
/// Interface that contains the data access classes for the replay
/// </summary>
public interface IReplayDataAccess
{
    public IBoardDataAccess BoardDataAccess { get; init; }
    public ILogFileDataAccess LogFileDataAccess { get; init; }
}