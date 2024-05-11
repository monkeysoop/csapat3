namespace Mekkdonalds.Persistence;

public class ReplayDataAccess : IReplayDataAccess
{
    public required IBoardDataAccess BoardDataAccess { get; init; }

    public required ILogFileDataAccess LogFileDataAccess { get; init; }

    private static ReplayDataAccess? _instance;

    public static ReplayDataAccess Instance
    {
        get
        {
            return _instance ??= new()
            {
                BoardDataAccess = new BoardFileDataAccess(),
                LogFileDataAccess = new LogFileDataAccess()
            };
        }
    }
}
