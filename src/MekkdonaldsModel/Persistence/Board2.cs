#define BORDER_CHECK

namespace MekkdonaldsModel.Persistence;

// Has to be public otherwise can't be used in Controller class
public class Board2
{
    #region Constants
    public const int EMPTY = 0;
    public const int WALL = 1;
    public const int SEARCHED = 2;
    public const int OPEN = 3;
    #endregion



    #region Fields
    private readonly int[] Data;
    public int Height { get; init; }
    public int Width { get; init; }
    #endregion



    #region Properties
    public int this[int y, int x]
    {
        get { return Data[y * Width + x]; }
        set { Data[y * Width + x] = value; }
    }

    public int this[Point p]
    {
        get { return Data[p.Y * Width + p.X]; }
        set { Data[p.Y * Width + p.X] = value; }
    }
    #endregion



    #region Constructors
    public Board2(int height, int width)
    {
        Data = new int[height * width];
        Height = height;
        Width = width;
    }

    public Board2(int[] data, int height, int width)
    {
        Data = new int[height * width];
        for (int i = 0; i < height * width; i++)
        {
            Data[i] = data[i];
        }
        Height = height;
        Width = width;
    }

    public Board2(int[,] data, int height, int width)
    {
        Data = new int[height * width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                this[y, x] = data[y, x];
            }
        }

        Height = height;
        Width = width;
    }
    #endregion



    #region Public methods
    public void SetSearched(Point position)
    {
#if NO_BORDER_CHECK
        bool t = true;
#else
        bool t = position.X >= 0 &&
                 position.X < Width &&
                 position.Y >= 0 &&
                 position.Y < Height;
#endif
        if (t)
        {
            this[position] = SEARCHED;
        }
    }

    public bool SetOpenIfEmpty(Point position)
    {
#if NO_BORDER_CHECK
        bool t = this[position] == EMPTY;
#else
        bool t = position.X >= 0 &&
                 position.X < Width &&
                 position.Y >= 0 &&
                 position.Y < Height &&
                 this[position] == EMPTY;
#endif
        if (t)
        {
            this[position] = OPEN;
        }

        return t;
    }

    public bool SetSearchedIfEmpty(Point position)
    {
#if NO_BORDER_CHECK
        bool t = this[position] == EMPTY;
#else
        bool t = position.X >= 0 &&
                 position.X < Width &&
                 position.Y >= 0 &&
                 position.Y < Height &&
                 this[position] == EMPTY;
#endif
        if (t)
        {
            this[position] = SEARCHED;
        }

        return t;
    }

    public static async Task<Board2> LoadAsync(string path)
    {
        try
        {
            using (StreamReader reader = new StreamReader(path)) // file opening
            {
                string line = await reader.ReadLineAsync() ?? string.Empty;
                string type = line.Split(' ')[1]; // saving the type

                line = await reader.ReadLineAsync() ?? string.Empty;
                int boardHeight = int.Parse(line.Split(' ')[1]); // read the height of the board

                line = await reader.ReadLineAsync() ?? string.Empty;
                int boardWidth = int.Parse(line.Split(' ')[1]); // read the widht of the board
                Board2 board = new(boardHeight, boardWidth); // creating the board

                line = await reader.ReadLineAsync() ?? string.Empty;

                if (line != "map")
                {
                    throw new Exception("Incorrect map file");
                }

                for (int h = 0; h < boardHeight; h++)
                {
                    line = await reader.ReadLineAsync() ?? string.Empty;

                    for (int w = 0; w < boardWidth; w++)
                    {
                        if (line[w].ToString() != ".")
                        {
                            board[h, w] = WALL;
                        }
                        else { board[h, w] = EMPTY; }
                    }
                }

                return board;
            }
        }
        catch
        {
            throw new Exception("Map loading error");
        }
    }

#endregion
}


