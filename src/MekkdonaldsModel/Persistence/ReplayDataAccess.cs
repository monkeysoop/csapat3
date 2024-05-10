namespace Mekkdonalds.Persistence;
/// <summary>
/// Data access for the replay
/// </summary>
public class ReplayDataAccess : IReplayDataAccess
{
    /// <summary>
    /// Data access for the board
    /// </summary>
    public required IBoardDataAccess BDA { get; init; }
    /// <summary>
    /// Data access for the log files
    /// </summary>
    public required ILogFileDataAccess LDA { get; init; }

    private static ReplayDataAccess? _instance;

    /// <summary>
    /// Creates a new instance of the ReplayDataAccess class
    /// </summary>
    public static ReplayDataAccess Instance
    {
        get
        {
            return _instance ??= new()
            {
                BDA = new BoardFileDataAccess(),
                LDA = new LogFileDataAccess()
            };
        }
    }
}
