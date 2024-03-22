namespace Mekkdonalds.Simulation;

public sealed class Robot : IMapObject
{
    private static readonly Dictionary<Direction, (int xd, int yd)> DirectionPoints = new()
    {
        { Direction.East, (1, 0) },
        { Direction.West, (-1, 0) },
        { Direction.North, (0, -1) },
        { Direction.South, (0, 1) }
    };

    private readonly List<Action> _history;

    public int ID { get; }
    public Point Start { get; }

    /// <summary>
    /// Current position of the robot
    /// </summary>
    public Point Position { get; private set; }

    /// <summary>
    /// The direction the robot is currently facing
    /// </summary>
    public Direction Direction { get; private set; }

    /// <summary>
    /// Task currently assigned to the robot
    /// </summary>
    public Package? Task { get; private set; }

    /// <summary>
    /// History of actions the robot executed
    /// </summary>
    public IReadOnlyList<Action> History
    {
        get => _history.AsReadOnly();
    }

    public Robot(int id, int x, int y)
    {
        ID = id;
        Start = Position = new Point(x, y);
        _history = [];
    }

    public Robot(int id) : this(id, 0, 0) { }

    // TODO: change this to internal
    public void Assign(int x, int y)
    {
        Task = new Package(x, y);
    }

    internal void Step(Action? t)
    {
        switch (t)
        {
            case Action.F:
                var (xd, yd) = DirectionPoints[Direction];

                var p = new Point(Position.X + xd, Position.Y + yd);

                Position = p;
                break;
            case Action.W:
                break;
            case Action.R:
                Direction = Direction.ClockWise();
                break;
            case Action.C:
                Direction = Direction.CounterClockWise();
                break;
            case Action.T: // I don't quite understand when this is supposed to occur but sure...
                break;
            case null:
                break;
            default:
                throw new System.Exception();
        }

        if (t is not null) _history.Add(t.Value);
    }
}
