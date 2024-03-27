namespace Mekkdonalds.Simulation;

public sealed class Robot(Point position, Direction direction) : IMapObject
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

    private Point _position = position;

    /// <summary>
    /// Current position of the robot
    /// </summary>
    public Point Position => _position;

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

    public void Step(Action a)
    {
        switch (a)
        {
            case Action.F: _position.Offset(position_offsets[(int)Direction]); break;
            case Action.R: Direction = Direction.ClockWise(); break;
            case Action.C: Direction = Direction.CounterClockWise(); break;
        }

        _history.Add(a);
    }
}
