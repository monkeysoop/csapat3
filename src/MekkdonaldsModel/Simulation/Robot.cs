namespace Mekkdonalds.Simulation;

public sealed class Robot : IMapObject
{
    private static readonly Point[] position_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];
    private static int IDCounter = 1;



    private readonly List<Action> _traversedRoute;
    private List<Action> _plannedRoute;
    private int _routeIndex;

    public int ID { get; }

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
        get => _traversedRoute.AsReadOnly();
    }

    public Robot(Point position, Direction direction)
    {
        Position = position;
        Direction = direction;

        ID = Robot.IDCounter;
        Robot.IDCounter++;

        _traversedRoute = [];
        _plannedRoute = [];
        _routeIndex = 0;
    }

    public bool Available()
    {
        return _routeIndex == _plannedRoute.Count;
    }

    public void AddPlannedRoute(List<Action> plannedRoute)
    {
        _plannedRoute = plannedRoute;
        _routeIndex = 0;
    }

    public void Wait()
    {
        _traversedRoute.Add(Action.W);
    }

    public void Step()
    {
        if (_routeIndex < _plannedRoute.Count)
        {
            Action a = _plannedRoute[_routeIndex];
            _routeIndex++;

            MakeStep(a);
            _traversedRoute.Add(a);
        }
    }

    private void MakeStep(Action a)
    {
        switch (a)
        {
            case Action.F: Position.Offset(position_offsets[(int)Direction]); break;
            case Action.R: Direction = Direction.ClockWise(); break;
            case Action.C: Direction = Direction.CounterClockWise(); break;
        }
    }
}
