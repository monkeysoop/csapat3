namespace Mekkdonalds.Persistence;

public interface IBoardDataAccess
{
    internal Task<Board> LoadAsync(string path);
    internal Task SaveAsync(string path, Board board);
}
