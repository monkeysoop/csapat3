namespace Mekkdonalds.Simulation;

public sealed class Robot(Point position, Direction direction)
{
    private static int IDCounter = 1;


    private readonly List<Action> _history = [];
    public int ID { get; } = IDCounter++;

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
    public Package RemoveTask()
    {
        if (Task == null)
        {
            throw new System.Exception("");
        }
        else
        {
            Package t = Task;
            Task = null;
            return t;
        }
    }
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

    public bool TryStep(Action a, Board board, int cost_counter)
    {
        switch (a)
        {
            case Action.F:
                Point next_position = Direction.GetNewOffsetPoint(Position);
                if (board.TryMoveRobot(Position, next_position))
                {
                    board.UnReserve(Position, cost_counter);
                    board.UnReserve(Position, cost_counter + 1);
                    Position = next_position;
                    _history.Add(a);
                    return true;
                }
                else
                {
                    // collision
                    // the robots planned path becomes invalid, so it should drop its path and unreserve all of it

                    //board.UnReserve(Position, cost_counter);
                    board.Reserve(Position, cost_counter + 1);
                    return false;
                }
            case Action.R:
                board.UnReserve(Position, cost_counter);
                Direction = Direction.ClockWise();
                _history.Add(a);
                return true;
            case Action.C:
                board.UnReserve(Position, cost_counter);
                Direction = Direction.CounterClockWise();
                _history.Add(a);
                return true;
            case Action.W:
                board.UnReserve(Position, cost_counter);
                _history.Add(a);
                return true;
            case Action.T:
                board.UnReserve(Position, cost_counter);
                board.Reserve(Position, cost_counter + 1);
                _history.Add(a);
                return false;
            default:
                throw new System.Exception("");
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
    }

    public static void ResetIDCounter()
    {
        IDCounter = 1;
    }
}
