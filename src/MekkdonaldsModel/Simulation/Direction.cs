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
    private static readonly Point[] position_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    public static Point GetOffset(this Direction direction) => position_offsets[(int)direction];

    public static Point GetNewOffsetPoint(this Direction direction, Point point) => new(point.X + position_offsets[(int)direction].X, point.Y + position_offsets[(int)direction].Y);

    public static Direction ClockWise(this Direction original) => (Direction)(((int)original + 1) % 4);

    public static Direction CounterClockWise(this Direction original) => (Direction)((3 + (int)original) % 4);

    public static Direction Opposite(this Direction original) => (Direction)((2 + (int)original) % 4);

    public static Direction StringToDirection(string direction)
    {
        return direction switch
        {
            "N" => Direction.North,
            "E" => Direction.East,
            "S" => Direction.South,
            "W" => Direction.West,
            _ => throw new ArgumentException("Unknown direction"),
        };
    }


}
