namespace Mekkdonalds.Persistence;
/// <summary>
/// Interface that contains the data access classes for the replay
/// </summary>
public interface IReplayDataAccess
{
    /// <summary>
    /// Data access for the board
    /// </summary>
    public IBoardDataAccess BoardDataAccess { get; init; }
    /// <summary>
    /// Data access for the log files
    /// </summary>
    public ILogFileDataAccess LogFileDataAccess { get; init; }
}
