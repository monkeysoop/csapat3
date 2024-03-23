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
}
