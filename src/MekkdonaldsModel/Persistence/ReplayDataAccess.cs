namespace Mekkdonalds.Persistence;

public class ReplayDataAccess : IReplayDataAccess
{
    public required IBoardDataAccess BDA { get; init; }

    public required ILogFileDataAccess LDA { get; init; }

    private static ReplayDataAccess? _instance;

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
