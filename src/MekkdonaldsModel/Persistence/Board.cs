#define NO_BORDER_CHECK

namespace Mekkdonalds.Persistence;

// Has to be public otherwise can't be used in Controller class
public class Board
{
    #region Constants
    public const int EMPTY = 0;
    public const int WALL = 1;
    public const int OCCUPIED = 1;
    public const int NOT_SEARCHED = 0;
    public const int SEARCHED = 1;

    public const int MAX_PATH_LENGTH_FACTOR = 20;
    #endregion



    #region Fields
    private readonly int[] Data;
    private readonly int[] SearchMask;
    private readonly int[] RobotMask;
    private readonly byte[] ReservationTable;
    public int MaxPathLength { get; init; }
    public int Height { get; init; }
    public int Width { get; init; }
    #endregion



    #region Constructors
    /// <summary>
    /// creates an empty board with given width and height and adds a border around it (so sizes are +2)
    /// </summary>
    /// <param name="height">The height of the board to be created not counting border (so -2 from the created height)</param>
    /// <param name="width">The width of the board to be created not counting border (so -2 from the created width)</param>
    public Board(int height, int width)
    {
        Height = height + 2;
        Width = width + 2;
        MaxPathLength = (Height + Width) * MAX_PATH_LENGTH_FACTOR;

        Data = new int[Height * Width];
        SearchMask = new int[Height * Width];
        RobotMask = new int[Height * Width];
        ReservationTable = new byte[Height * Width * MaxPathLength]; // lots and lots of memory

        AddBorder();
    }

    /// <summary>
    /// creates a board with given width and height and given data and adds a border around it (so sizes are +2)
    /// </summary>
    /// <param name="data">2D matrix where each item is either a wall or empty</param>
    /// <param name="height">The height of the board to be created not counting border (so -2 from the created height)</param>
    /// <param name="width">The width of the board to be created not counting border (so -2 from the created width)</param>
    public Board(int[,] data, int height, int width)
    {
        Height = height + 2;
        Width = width + 2;
        MaxPathLength = (Height + Width) * MAX_PATH_LENGTH_FACTOR;

        Data = new int[Height * Width];
        SearchMask = new int[Height * Width];
        RobotMask = new int[Height * Width];
        ReservationTable = new byte[Height * Width * MaxPathLength]; // lots and lots of memory

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                SetValue((x + 1), (y + 1), data[y, x]); // this checks the input from data[,]
            }
        }
        AddBorder();
    }
    #endregion



    #region Public methods
    /// <summary>
    /// Tries to move a robot
    /// </summary>
    /// <param name="currentPosition">Current position of the robot</param>
    /// <param name="nextPosition">Position to be moved into</param>
    /// <returns>True if the move was successful false if couldn't move it because of another robot blocking its way</returns>
    public bool TryMoveRobot(Point currentPosition, Point nextPosition)
    {
        CheckPosition(currentPosition);
        CheckPosition(nextPosition);

        if (RobotMask[nextPosition.Y * Width + nextPosition.X] == EMPTY)
        {
            RobotMask[currentPosition.Y * Width + currentPosition.X] = EMPTY;
            RobotMask[nextPosition.Y * Width + nextPosition.X] = OCCUPIED;
            return true;
        }
        return false;
    }

    /// <summary>
    /// un-reserves a position for a certain time, it wraps around so it is only effective for paths/times under the max path length
    /// </summary>
    /// <param name="position">Position of the reservation to be removed</param>
    /// <param name="cost">Time of the reservation to be removed</param>
    public void UnReserve(Point position, int cost)
    {
        CheckPosition(position);
        CheckCost(cost);

        ReservationTable[(position.Y * Width + position.X) * MaxPathLength + ((cost + MaxPathLength) % MaxPathLength)] = EMPTY;
    }

    /// <summary>
    /// reserves a position for a certain time, it wraps around so it is only effective for paths/times under the max path length
    /// </summary>
    /// <param name="position">Position of the reservation</param>
    /// <param name="cost">Time of the reservation</param>
    public void Reserve(Point position, int cost)
    {
        CheckPosition(position);
        CheckCost(cost);

        ReservationTable[(position.Y * Width + position.X) * MaxPathLength + ((cost + MaxPathLength) % MaxPathLength)] = OCCUPIED;
    }

    /// <summary>
    /// Checks if the next position for a given time is not reserved
    /// </summary>
    /// <param name="nextPosition">Next position</param>
    /// <param name="cost">Time</param>
    /// <returns>True if it's not reserved otherwise false</returns>
    public bool NotReservedForward(Point nextPosition, int cost)
    {
        CheckPosition(nextPosition);
        CheckCost(cost);

        return (ReservationTable[(nextPosition.Y * Width + nextPosition.X) * MaxPathLength + (cost % MaxPathLength)] == EMPTY);
    }

    /// <summary>
    /// Checks if the next position for a given time is not reserved and also it can stay for 1 more time step to turn towards the next position
    /// </summary>
    /// <param name="currentPosition">Current position</param>
    /// <param name="nextPosition">Next position</param>
    /// <param name="cost">Time</param>
    /// <returns>True if it's not reserved both at the current and next position</returns>
    public bool NotReservedLeftRight(Point currentPosition, Point nextPosition, int cost)
    {
        CheckPosition(currentPosition);
        CheckPosition(nextPosition);
        CheckCost(cost - 1);

        return (ReservationTable[(nextPosition.Y * Width + nextPosition.X) * MaxPathLength + (cost % MaxPathLength)] == EMPTY) &&
               (ReservationTable[(currentPosition.Y * Width + currentPosition.X) * MaxPathLength + ((cost + MaxPathLength - 1) % MaxPathLength)] == EMPTY);
    }

    /// <summary>
    /// Checks and sets the search mask searched if the next position for a given time is not reserved and is not a wall, not another stationary robot and wasn't searched before
    /// </summary>
    /// <param name="nextPosition">Next position</param>
    /// <param name="cost">Time</param>
    /// <returns>True if the next position is searchable otherwise false</returns>
    public bool SetSearchedIfEmptyForward(Point nextPosition, int cost)
    {
        CheckPosition(nextPosition);
        CheckCost(cost);

        bool t = (Data[nextPosition.Y * Width + nextPosition.X] == EMPTY) &&
                 (RobotMask[nextPosition.Y * Width + nextPosition.X] == EMPTY) && // this is needed, because the reservation table cant really be set when a robot finishes a task and doesn't immediately get a new one
                 (SearchMask[nextPosition.Y * Width + nextPosition.X] == NOT_SEARCHED) &&
                 (ReservationTable[(nextPosition.Y * Width + nextPosition.X) * MaxPathLength + (cost % MaxPathLength)] == EMPTY);

        if (t)
        {
            SearchMask[nextPosition.Y * Width + nextPosition.X] = SEARCHED;
        }

        return t;
    }

    /// <summary>
    /// Checks and sets the search mask searched if the next position for a given time is not reserved and the current position is not reserved for 1 turning step and is not a wall, not another stationary robot and wasn't searched before
    /// </summary>
    /// <param name="currentPosition">Current position</param>
    /// <param name="nextPosition">Next position</param>
    /// <param name="cost">Time</param>
    /// <returns>True if the next position is searchable otherwise false</returns>
    public bool SetSearchedIfEmptyLeftRight(Point currentPosition, Point nextPosition, int cost)
    {
        CheckPosition(currentPosition);
        CheckPosition(nextPosition);
        CheckCost(cost - 1);

        bool t = (Data[nextPosition.Y * Width + nextPosition.X] == EMPTY) &&
                 (RobotMask[nextPosition.Y * Width + nextPosition.X] == EMPTY) && // this is needed, because the reservation table cant really be set when a robot finishes a task and doesn't immediately get a new one
                 (SearchMask[nextPosition.Y * Width + nextPosition.X] == NOT_SEARCHED) &&
                 (ReservationTable[(currentPosition.Y * Width + currentPosition.X) * MaxPathLength + ((cost + MaxPathLength - 1) % MaxPathLength)] == EMPTY) &&
                 (ReservationTable[(nextPosition.Y * Width + nextPosition.X) * MaxPathLength + ((cost) % MaxPathLength)] == EMPTY);

        if (t)
        {
            SearchMask[nextPosition.Y * Width + nextPosition.X] = SEARCHED;
        }

        return t;
    }

    /// <summary>
    /// Checks and sets the search mask searched if the next position for a given time is not reserved and the current position is not reserved for 1 turning step and is not a wall, not another stationary robot and wasn't searched before
    /// </summary>
    /// <param name="currentPosition">Current position</param>
    /// <param name="nextPosition">Next position</param>
    /// <param name="cost">Time</param>
    /// <returns>True if the next position is searchable otherwise false</returns>
    public bool SetSearchedIfEmptyBackward(Point currentPosition, Point nextPosition, int cost)
    {
        CheckPosition(currentPosition);
        CheckPosition(nextPosition);
        CheckCost(cost - 2);

        bool t = //(Data[current_position.Y * Width + current_position.X] == EMPTY) && this is checked by SetSearchedIfEmptyStart and exception is thrown if needed
                 (Data[nextPosition.Y * Width + nextPosition.X] == EMPTY) &&
                 (RobotMask[nextPosition.Y * Width + nextPosition.X] == EMPTY) && // this is needed, because the reservation table cant really be set when a robot finishes a task and doesn't immediately get a new one
                 (SearchMask[nextPosition.Y * Width + nextPosition.X] == NOT_SEARCHED) &&
                 //(ReservationTable[(current_position.Y * Width + current_position.X) * MaxPathLength + ((cost + MaxPathLength - 2) % MaxPathLength)] == EMPTY) &&
                 (ReservationTable[(currentPosition.Y * Width + currentPosition.X) * MaxPathLength + ((cost + MaxPathLength - 1) % MaxPathLength)] == EMPTY) &&
                 (ReservationTable[(nextPosition.Y * Width + nextPosition.X) * MaxPathLength + ((cost) % MaxPathLength)] == EMPTY);

        if (t)
        {
            SearchMask[nextPosition.Y * Width + nextPosition.X] = SEARCHED;
        }

        return t;
    }

    /// <summary>
    /// Checks and sets the search mask searched if the current position (start) is not a wall
    /// </summary>
    /// <param name="currentPosition">Current position</param>
    /// <param name="cost">Time</param>
    /// <returns>True if the next position is searchable otherwise false</returns>
    public bool SetSearchedIfEmptyStart(Point currentPosition, int cost)
    {
        CheckPosition(currentPosition);
        CheckCost(cost);

        bool t = (Data[currentPosition.Y * Width + currentPosition.X] == EMPTY);

        if (t)
        {
            SearchMask[currentPosition.Y * Width + currentPosition.X] = SEARCHED;
        }

        return t;
    }

    /// <summary>
    /// Checks if the given position has been marked as searched
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool Searchable(Point position)
    {
        CheckPosition(position);

        return SearchMask[position.Y * Width + position.X] == SEARCHED;
    }

    public bool SetSearchedIfEmpty(Point position)
    {
        CheckPosition(position);

#if NO_BORDER_CHECK
        bool t = Data[position.Y * Width + position.X] == EMPTY &&
                 SearchMask[position.Y * Width + position.X] == NOT_SEARCHED &&
                 RobotMask[position.Y * Width + position.X] == EMPTY;
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

    /// <summary>
    /// Clears the search mask to not searched
    /// </summary>
    public void ClearMask()
    {
        for (int i = 0; i < Height * Width; i++)
        {
            SearchMask[i] = NOT_SEARCHED;
        }
    }

    public void SetValue(int x, int y, int value)
    {
        CheckPosition(x, y);
        CheckValue(value);

        Data[y * Width + x] = value;
    }

    public int GetValue(int x, int y)
    {
        CheckPosition(x, y);

        return Data[y * Width + x];
    }
    // only for testing
    public int GetSearchMaskValue(int x, int y)
    {
        return SearchMask[y * Width + x];
    }

    public void SetSearchMaskValue(int x, int y, int value)
    {
        SearchMask[y * Width + x] = value;
    }
    public int GetRobotMaskValue(int x, int y)
    {
        return RobotMask[y * Width + x];
    }

    public void SetRobotMaskValue(int x, int y, int value)
    {
        RobotMask[y * Width + x] = value;
    }
    #endregion



    #region Private methods
    /// <summary>
    /// Adds a 1 thick border (of type wall) around the map
    /// </summary>
    private void AddBorder()
    {
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                Data[x] = WALL;
                Data[(Height - 1) * Width + x] = WALL;
                Data[y * Width] = WALL;
                Data[y * Width + Width - 1] = WALL;
            }
        }
    }
    private void CheckPosition(Point position)
    {
#if DEBUG
        if (position.X < 0 || position.X >= Width || position.Y < 0 || position.Y >= Height)
        {
            Debug.WriteLine("invalid position: " + position);
            throw new System.Exception("invalid position" + position);
        }
#endif
    }

    private void CheckPosition(int x, int y)
    {
#if DEBUG
        if (x < 0 || x >= Width || y < 0 || y >= Height)
        {
            Debug.WriteLine("invalid position: " + new Point(x, y));
            throw new System.Exception("invalid position" + new Point(x, y));
        }
#endif
    }

    private static void CheckCost(int cost)
    {
#if DEBUG
        if (cost < 0)
        {
            Debug.WriteLine("invalid cost: " + cost);
            throw new System.Exception("invalid cost" + cost);
        }
#endif
    }

    private static void CheckValue(int value)
    {
#if DEBUG
        if (value != EMPTY && value != WALL)
        {
            Debug.WriteLine("invalid value: " + value);
            throw new System.Exception("invalid value" + value);
        }
#endif
    }
    #endregion
}


