namespace Mekkdonalds.Simulation;

internal enum Direction
{
    North,
    East,
    South,
    West
}

internal static class DirectionMethods
{
    internal static Direction ClockWise(this Direction original) => (Direction)(((int)original + 1) % 4);

    internal static Direction CounterClockWise(this Direction original) => (Direction)((3 + (int)original) % 4);
}
