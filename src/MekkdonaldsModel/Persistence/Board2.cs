#define BORDER_CHECK

namespace MekkdonaldsModel.Persistence;

// Has to be public otherwise can't be used in Controller class
public class Board2
{
    #region Constants
    public const int EMPTY = 0;
    public const int WALL = 1;
    public const int SEARCHED = 1;
    //public const int OPEN = 3;
    #endregion



    #region Fields
    private readonly int[] Data;
    private readonly int[] SearchMask;
    public int Height { get; init; }
    public int Width { get; init; }
    #endregion



    //#region Properties
    //public int this[int y, int x] { get {return GetValue(x, y);} set {SetValue(x, y, value);} }
    //public int this[Point p] { get {return GetValue(p.X, p.Y);} set {SetValue(p.X, p.Y, value);} }
    //#endregion



    #region Constructors
    public Board2(int height, int width)
    {
        Height = height;
        Width = width;

        Data = new int[height * width];

        SearchMask = new int[height * width];
    }

    public Board2(int[,] data, int height, int width)
    {
        Height = height;
        Width = width;
        
        Data = new int[height * width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Data[y * width + x] = data[y, x];
            }
        }

        SearchMask = new int[height * width];
    }
    #endregion



    #region Public methods
    public bool SetSearchedIfEmpty(Point position)
    {
#if NO_BORDER_CHECK
        bool t = (Data[position.Y * Width + Height] == EMPTY);
#else
        bool t = position.X >= 0 &&
                 position.X < Width &&
                 position.Y >= 0 &&
                 position.Y < Height &&
                 Data[position.Y * Width + Height] == EMPTY;
#endif
        if (t)
        {
            SearchMask[position.Y * Width + Height] = SEARCHED;
        }

        return t;
    }

    public void ClearMask()
    {
        for (int i = 0; i < Height * Width; i++)
        {
            SearchMask[i] = 0;
        }
    }

    public void SetValue(int x, int y, int value)
    {
        if (x < 0 || x >= this.Height)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
        }
        if (y < 0 || y >= this.Width)
        {
            throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
        }
        if (value < 0 || value > 1)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "The value is out of range.");
        }

        Data[y * Width + x] = value;
    }

    public int GetValue(int x, int y)
    {
        if (x < 0 || x >= this.Height)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
        }
        if (y < 0 || y >= this.Width)
        {
            throw new ArgumentOutOfRangeException(nameof(y), "The Y coordinate is out of range.");
        }

        return Data[y * Width + x];
    }

    public async Task<Board2> LoadAsync(String path)
    {
        try
        {
            using (StreamReader reader = new StreamReader(path)) // file opening
            {
                String line = await reader.ReadLineAsync() ?? String.Empty;
                String type = line.Split(' ')[1]; // saving the type

                line = await reader.ReadLineAsync() ?? String.Empty;
                Int32 boardHeight = Int32.Parse(line.Split(' ')[1]); // read the height of the board

                line = await reader.ReadLineAsync() ?? String.Empty;
                Int32 boardWidth = Int32.Parse(line.Split(' ')[1]); // read the widht of the board
                Board2 board = new Board2(boardHeight, boardWidth); // creating the board

                line = await reader.ReadLineAsync() ?? String.Empty;

                if (line != "map")
                {
                    throw new Exception("Incorrect map file");
                }

                for (Int32 h = 0; h < boardHeight; h++)
                {
                    line = await reader.ReadLineAsync() ?? String.Empty;

                    for (Int32 w = 0; w < boardWidth; w++)
                    {
                        if (line[w].ToString() != ".")
                        {
                            board.SetValue(h, w, WALL);
                        }
                        else { board.SetValue(h, w, EMPTY); }
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


