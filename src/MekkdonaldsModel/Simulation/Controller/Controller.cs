namespace Mekkdonalds.Simulation.Controller;

public abstract class Controller
{
    protected ConcurrentDictionary<Robot, Path> Paths;
    protected List<Robot> _robots;
    protected List<Wall> _walls;
    protected Timer Timer;

    protected Board2 _board;

    public IReadOnlyList<Robot> Robots => _robots.AsReadOnly();

    public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

    public event EventHandler? Tick;

    public Controller()
    {
        Paths = [];
        _robots = [];
        _walls = [];

        _board = new(0, 0);

        Timer = new Timer(OnTick, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan); // this is probably better then System.Timers.Timer (it is already asynchronous)
    }

    protected abstract void OnTick(object? state);

    protected void CallTick(object? sender)
    {
        Tick?.Invoke(sender, EventArgs.Empty);
    }
}