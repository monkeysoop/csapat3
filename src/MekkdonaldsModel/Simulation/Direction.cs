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
    private static readonly Point[] _positionOffsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    public static Point GetOffset(this Direction direction) => _positionOffsets[(int)direction];

    public static Point GetOffset(this int direction) => _positionOffsets[direction];

    public static Point GetNewOffsetPoint(this Direction direction, Point point) => new(point.X + _positionOffsets[(int)direction].X, point.Y + _positionOffsets[(int)direction].Y);

    public static Direction ClockWise(this Direction original) => (Direction)(((int)original + 1) % 4);

    public static Direction CounterClockWise(this Direction original) => (Direction)((3 + (int)original) % 4);

    public static Direction Opposite(this Direction original) => (Direction)((2 + (int)original) % 4);

    public static Direction Parse(string direction)
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
