namespace Mekkdonalds.Simulation;

public sealed class Robot(Point position, Direction direction)
{
    private static readonly Point[] position_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];
    private static int IDCounter = 1;



    private readonly List<Action> _history = [];
    public int ID { get; } = IDCounter++;

    //public Point Position = position;

    /// <summary>
    /// Current position of the robot
    /// </summary>
    public Point Position { get; private set; } = position;

    /// <summary>
    /// The direction the robot is currently facing
    /// </summary>
    public Direction Direction { get; private set; } = direction;

    /// <summary>
    /// Task currently assigned to the robot
    /// </summary>
    public Package? Task { get; private set; }

    /// <summary>
    /// History of actions the robot executed
    /// </summary>
    public IReadOnlyList<Action> History => _history.AsReadOnly();

    public void AddTask(Point? p)
    {
        if (p is null)
        {
            Task = null;
            return;
        }

        Task = new Package(p.Value);
    }

    public void AddTask(Package? p)
    {
        Task = p;
    }

    public bool TryStep(Action a, Board board)
    {
        switch (a)
        {
            case Action.F:
                Point next_position = Direction.GetNewOffsetPoint(Position);
                if (board.TryMoveRobot(Position, next_position))
                {
                    Position = next_position;
                    return true;
                }
                return false;
            case Action.R:
                Direction = Direction.ClockWise();
                return true;
            case Action.C:
                Direction = Direction.CounterClockWise();
                return true;
            case Action.W:
                return true;
            case Action.T:
                return false;
            default:
                return false;
        }
    }

    public void Step(Action a)
    {
        switch (a)
        {
            case Action.F: Position = Direction.GetNewOffsetPoint(Position); break;
            case Action.R: Direction = Direction.ClockWise(); break;
            case Action.C: Direction = Direction.CounterClockWise(); break;
            case Action.W: break;
            case Action.B: Position = Direction.Opposite().GetNewOffsetPoint(Position); break;
            default:
                throw new System.Exception("");
        }

        _history.Add(a);
    }
}
