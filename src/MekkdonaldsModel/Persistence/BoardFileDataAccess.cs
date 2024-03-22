namespace MekkdonaldsModel.Persistence;

internal class BoardFileDataAccess : IBoardDataAccess
{
    #region Constants
    public const int EMPTY = 0;
    public const int WALL = 1;
    public const char EMPTY_CHAR = '.';
    public const char WALL_CHAR = '@';
    #endregion



    public async Task<Board2> LoadAsync(string path)
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

            return new Board2(data, boardHeight, boardWidth);
        }
        catch
        {
            throw new Exception("Map loading error");
        }
    }

    public async Task SaveAsync(string path, Board2 board)
    {
        await Task.Delay(0);

        throw new NotImplementedException();
    }
}
