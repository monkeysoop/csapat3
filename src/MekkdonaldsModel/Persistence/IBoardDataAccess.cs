namespace Mekkdonalds.Persistence;
/// <summary>
/// Interface for the board data access
/// </summary>
public interface IBoardDataAccess
{
    /// <summary>
    /// Load a board from a file
    /// </summary>
    /// <param name="path">path to the file</param> 
    /// <returns>task that represents the loading operation. The task result contains the board</returns> 
    internal Task<Board> LoadAsync(string path);
    /// <summary>
    /// Save a board to a file
    /// </summary>
    /// <param name="path"> path to the file</param>
    /// <param name="board">board to save</param> 
    /// <returns>task that represents the saving operation</returns> 
    internal Task SaveAsync(string path, Board board);
}
