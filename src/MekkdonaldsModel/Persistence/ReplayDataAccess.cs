namespace Mekkdonalds.Persistence;

public class ReplayDataAccess : IReplayDataAccess
{
    public required IBoardDataAccess BDA { get; init; }
}
