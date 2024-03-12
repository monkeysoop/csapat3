

namespace MekkdonaldsModel.Persistence
{
    internal interface IBoardDataAccess
    {
        internal Task<Board2> LoadAsync(string path);
        internal Task SaveAsync(string path, Board2 board);
    }
}
