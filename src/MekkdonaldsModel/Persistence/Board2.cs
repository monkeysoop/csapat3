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
        get
        {
            if (y < 0 || x < 0 || y >= Height || x >= Width) { throw new BoardDataException(); }
            return Data[y * Width + x];
        }
        set
        {
            if (y < 0 || x < 0 || y >= Height || x >= Width) { throw new BoardDataException(); }
            Data[y * Width + x] = value;
        }
    }

    public int this[Point p]
    {
        get
        {
            if (p.Y < 0 || p.X < 0 || p.Y >= Height || p.X >= Width) { throw new BoardDataException(); }
            return Data[p.Y * Width + p.X];
        }
        set
        {
            if (p.Y < 0 || p.X < 0 || p.Y >= Height || p.X >= Width) { throw new BoardDataException(); }
            Data[p.Y * Width + p.X] = value;
        }
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


    public void SetValue(Int32 X, Int32 Y, Int32 value)
    {
        if (X < 0 || X >= this.Height)
            throw new ArgumentOutOfRangeException(nameof(X), "The X coordinate is out of range.");
        if (Y < 0 || Y >= this.Width)
            throw new ArgumentOutOfRangeException(nameof(Y), "The Y coordinate is out of range.");
        if (value < 0 || value > 3)
            throw new ArgumentOutOfRangeException(nameof(value), "The value is out of range.");

        Data[Y * Width + X] = value;
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


