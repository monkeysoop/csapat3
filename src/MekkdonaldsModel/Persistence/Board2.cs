#define BORDER_CHECK

namespace Mekkdonalds.Persistence;

// Has to be public otherwise can't be used in Controller class
public class Board2
{
    #region Constants
    public const int EMPTY = 0;
    public const int WALL = 1;
    public const int NOT_SEARCHED = 0;
    public const int SEARCHED = 1;
    #endregion



    #region Fields
    public readonly int[] Data;
    private readonly int[] SearchMask;
    public int Height { get; init; }
    public int Width { get; init; }
    #endregion



    #region Constructors
    public Board2(int height, int width)
    {
        Height = height;
        Width = width;

        Data = new int[height * width];
        SearchMask = new int[height * width];

        for (int i = 0; i < height * width; i++)
        {
            Data[i] = EMPTY;
            SearchMask[i] = NOT_SEARCHED; 
        }
    }

    public Board2(int[,] data, int height, int width)
    {
        Height = height;
        Width = width;
        
        Data = new int[height * width];
        SearchMask = new int[height * width];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                SetValue(x, y, data[y, x]); // this checks the input from data[,]
                SearchMask[y * width + x] = NOT_SEARCHED;
            }
        }

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
                 Data[position.Y * Width + position.X] == EMPTY &&
                 SearchMask[position.Y * Width + position.X] == NOT_SEARCHED;
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
            SearchMask[i] = NOT_SEARCHED;
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
        if (value != EMPTY && value != WALL)
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


