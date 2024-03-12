#define BORDER_CHECK

namespace MekkdonaldsModel.Persistence;

// Has to be public otherwise can't be used in Controller class
public class Board2
{
    public const int EMPTY = 0;
    public const int WALL = 1;
    public const int SEARCHED = 2;
    public const int OPEN = 3;

    private readonly int[] Data;

    public int Height { get; init; }
    public int Width { get; init; }



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
                Data[y * width + x] = data[y, x]; 
            }
        }

        Height = height;
        Width = width;
    }

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
            Data[position.Y * Width + position.X] = SEARCHED;
        }
    }

    public bool SetOpenIfEmpty(Point position)
    {
#if NO_BORDER_CHECK
        bool t = data[position.Y * width + position.X] == EMPTY;
#else
        bool t = position.X >= 0 &&
                 position.X < Width &&
                 position.Y >= 0 &&
                 position.Y < Height &&
                 Data[position.Y * Width + position.X] == EMPTY;
#endif
        if (t)
        {
            Data[position.Y * Width + position.X] = OPEN;
        }

        return t;
    }

    public bool SetSearchedIfEmpty(Point position)
    {
#if NO_BORDER_CHECK
        bool t = data[position.Y * width + position.X] == EMPTY;
#else
        bool t = position.X >= 0 &&
                 position.X < Width &&
                 position.Y >= 0 &&
                 position.Y < Height &&
                 Data[position.Y * Width + position.X] == EMPTY;
#endif
        if (t)
        {
            Data[position.Y * Width + position.X] = SEARCHED;
        }

        return t;
    }
}