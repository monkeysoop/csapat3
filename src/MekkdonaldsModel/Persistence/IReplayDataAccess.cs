namespace Mekkdonalds.Persistence;

public interface IReplayDataAccess
{
    /// <summary>
    /// data access for the board
    /// </summary>
    public IBoardDataAccess BDA { get; init; }
    /// <summary>
    /// data access for the log files
    /// </summary>
    public ILogFileDataAccess LDA { get; init; }
}