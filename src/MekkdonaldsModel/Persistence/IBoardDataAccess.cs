namespace Mekkdonalds.Persistence;
/// <summary>
/// Interface for the board data access
/// </summary>
public interface IBoardDataAccess
{
    /// <summary>
    /// Load a board from a file
    /// </summary>
    /// <param name="path">Path to the file</param> 
    /// <returns>Task that represents the loading operation. The task result contains the board</returns> 
    internal Task<Board> LoadAsync(string path);
    /// <summary>
    /// Save a board to a file
    /// </summary>
    /// <param name="path"> Path to the file</param>
    /// <param name="board">Board to save</param> 
    /// <returns>Task that represents the saving operation</returns> 
    internal Task SaveAsync(string path, Board board);
}
