namespace Mekkdonalds.Persistence;

public class BoardFileDataAccess : IBoardDataAccess
{
    #region Constants
    public const int EMPTY = 0;
    public const int WALL = 1;
    public const char EMPTY_CHAR = '.';
    #endregion
    /// <summary>
    /// loads the board from the file
    /// </summary>
    /// <param name="path"> path to the file</param>
    /// <returns>task that represents the loading operation. The task result contains the board</returns> 
    /// <exception cref="BoardDataException">to be thrown when the data is incorrect</exception> 
    public async Task<Board> LoadAsync(string path)
    {
        try
        {
            using StreamReader reader = new(path); // file opening

            string line = await reader.ReadLineAsync() ?? string.Empty;
            string type = line.Split(' ')[1]; // saving the type

            line = await reader.ReadLineAsync() ?? string.Empty;

            if (!int.TryParse(line.Split(' ')[1], out int boardHeight) || boardHeight < 1)
                throw new BoardDataException("Invalid board height");

            line = await reader.ReadLineAsync() ?? string.Empty;

            if (!int.TryParse(line.Split(' ')[1], out int boardWidth) || boardWidth < 1)
                throw new BoardDataException("Invalid board width");

            line = await reader.ReadLineAsync() ?? string.Empty;

            if (line != "map")
            {
                throw new BoardDataException("Incorrect identifier (expected: map)");
            }

            int[,] data = new int[boardHeight, boardWidth];

            for (int y = 0; y < boardHeight; y++)
            {
                line = await reader.ReadLineAsync() ?? string.Empty;

                for (int x = 0; x < boardWidth; x++)
                {
                    if (line[x] == EMPTY_CHAR)
                    {
                        data[y, x] = EMPTY;
                    }
                    else
                    {
                        data[y, x] = WALL;
                    }
                }
            }

            return new Board(data, boardHeight, boardWidth);
        }
        catch (System.Exception e)
        {
            throw new BoardDataException("Map loading error", e);
        }
    }
    /// <summary>
    /// saves the board to the file
    /// </summary>
    /// <param name="path">path to the file</param> 
    /// <param name="board"> board to save</param>
    /// <returns>task that represents the saving operation</returns> 
    /// <exception cref="NotImplementedException">thrown when the method is not implemented</exception> 
    public async Task SaveAsync(string path, Board board)
    {
        await Task.Delay(0);

        throw new NotImplementedException();
    }
}
