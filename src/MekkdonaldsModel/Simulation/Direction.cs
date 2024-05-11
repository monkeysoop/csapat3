namespace Mekkdonalds.Simulation;

/// <summary>
/// Directions
/// </summary>
public enum Direction
{
    North = 0,
    East = 1,
    South = 2,
    West = 3
}

/// <summary>
/// Extension methods for the <see cref="Direction"/> enum
/// </summary>
public static class DirectionMethods
{
    private static readonly Point[] _positionOffsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    /// <summary>
    /// Get the offset for a direction ((0, -1) for North, (1, 0) for East, (0, 1) for South, (-1, 0) for West)
    /// </summary>
    /// <param name="direction">The direction</param>
    /// <returns>The point offset</returns>
    public static Point GetOffset(this Direction direction) => _positionOffsets[(int)direction];

    /// <summary>
    /// Get the offset for the value of a direction. See also <see cref="GetOffset(Direction)"/>
    /// </summary>
    /// <param name="direction">The value of the direction</param>
    /// <returns>The point offset</returns>
    public static Point GetOffset(this int direction) => _positionOffsets[direction];

    /// <summary>
    /// Adds the offset of a direction to a point
    /// </summary>
    /// <param name="direction">Direction to move in</param>
    /// <param name="point">The origin point</param>
    /// <returns>The point with the offset</returns>
    public static Point GetNewOffsetPoint(this Direction direction, Point point) => new(point.X + _positionOffsets[(int)direction].X, point.Y + _positionOffsets[(int)direction].Y);

    /// <summary>
    /// Rotates a direction 90 degrees clockwise
    /// </summary>
    /// <param name="original">Direction to rotate</param>
    /// <returns>Rotated direction</returns>
    public static Direction ClockWise(this Direction original) => (Direction)(((int)original + 1) % 4);

    /// <summary>
    /// Rotates a direction 90 degrees counter clockwise
    /// </summary>
    /// <param name="original">Direction to rotate</param>
    /// <returns>Rotated direction</returns>
    public static Direction CounterClockWise(this Direction original) => (Direction)((3 + (int)original) % 4);

    /// <summary>
    /// Rotates a direction 180 degrees
    /// </summary>
    /// <param name="original">Direction to rotate</param>
    /// <returns>Rotated direction</returns>
    public static Direction Opposite(this Direction original) => (Direction)((2 + (int)original) % 4);

    internal static Direction Parse(string direction)
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
