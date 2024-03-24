#define BORDER_CHECK

namespace Mekkdonalds.Persistence;

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
                SetValue(x, y, data[y, x]); // this checks the input from data[,]
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
                 Data[position.Y * Width + position.X] == EMPTY;
#endif
        if (t)
        {
            SearchMask[position.Y * Width + position.X] = SEARCHED;
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
        if (x < 0 || x >= this.Width)
        {
            throw new ArgumentOutOfRangeException(nameof(x), "The X coordinate is out of range.");
        }
        if (y < 0 || y >= this.Height)
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
    #endregion
}


