namespace Mekkdonalds.Simulation;

public class Robot : IMapObject
{
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
    public Task? Task { get; private set; }
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
        Task = new Task(x, y);
    }
}
