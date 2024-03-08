namespace Mekkdonalds.Simulation;

public enum Direction
{
    North,
    East,
    South,
    West
}

public static class DirectionMethods
{
    public static Direction ClockWise(this Direction original) => (Direction)(((int)original + 1) % 4);

    public static Direction CounterClockWise(this Direction original) => (Direction)((3 + (int)original) % 4);
}
