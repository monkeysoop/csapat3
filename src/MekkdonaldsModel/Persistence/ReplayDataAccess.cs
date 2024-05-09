namespace Mekkdonalds.Persistence;

public class ReplayDataAccess : IReplayDataAccess
{
    /// <summary>
    /// data access for the board
    /// </summary>
    public required IBoardDataAccess BDA { get; init; }
    /// <summary>
    /// data access for the log files
    /// </summary>
    public required ILogFileDataAccess LDA { get; init; }

    private static ReplayDataAccess? _instance;

    /// <summary>
    /// creates a new instance of the ReplayDataAccess class
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
