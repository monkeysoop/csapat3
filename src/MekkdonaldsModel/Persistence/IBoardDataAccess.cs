namespace Mekkdonalds.Persistence;

public interface IBoardDataAccess
{
    internal Task<Board2> LoadAsync(string path);
    internal Task SaveAsync(string path, Board2 board);
}
