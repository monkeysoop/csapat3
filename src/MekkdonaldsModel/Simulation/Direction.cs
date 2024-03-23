using static System.Collections.Specialized.BitVector32;

namespace Mekkdonalds.Simulation;

public enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

public static class DirectionMethods
{
    public static Direction ClockWise(this Direction original) => (Direction)(((int)original + 1) % 4);

    public static Direction CounterClockWise(this Direction original) => (Direction)((3 + (int)original) % 4);

    public static string DirectionToString(this Direction original)
    {
        switch (original)
        {
            case Direction.North:
                return "N";
            case Direction.East:
                return "E";
            case Direction.South:
                return "S";
            case Direction.West:
                return "W";
            default:
                return original.ToString();
        }
    }

    public static Direction StringToDirection(string original)
    {
        switch (original)
        {
            case "N":
                return Direction.North;
            case "E":
                return Direction.East;
            case "S":
                return Direction.South;
            case "W":
                return Direction.West;
            default:
                throw new ArgumentException();
        }
    }


}
