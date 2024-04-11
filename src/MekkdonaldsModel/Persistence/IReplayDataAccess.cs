namespace Mekkdonalds.Persistence;

public interface IReplayDataAccess
{
    public IBoardDataAccess BDA { get; init; }
    public ILogFileDataAccess LDA { get; init; }
}