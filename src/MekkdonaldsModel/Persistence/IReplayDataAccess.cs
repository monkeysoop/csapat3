namespace Mekkdonalds.Persistence;
/// <summary>
/// Interface for the replay data access
/// </summary>
public interface IReplayDataAccess
{
    /// <summary>
    /// Data access for the board
    /// </summary>
    public IBoardDataAccess BDA { get; init; }
    /// <summary>
    /// Data access for the log files
    /// </summary>
    public ILogFileDataAccess LDA { get; init; }
}