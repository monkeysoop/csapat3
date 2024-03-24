namespace Mekkdonalds.Simulation.Controller;

public abstract class Controller
{
    protected ConcurrentDictionary<Robot, Path> Paths;
    protected List<Robot> _robots;
    protected List<Wall> _walls;
    protected List<Package> _packages;
    protected Timer Timer;
    private readonly TimeSpan _interval;

    protected Board2 _board;

    public int Width => _board.Width;
    public int Height => _board.Height;

    public IReadOnlyList<Robot> Robots => _robots.AsReadOnly();

    public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

    public event EventHandler? Tick;
    public event EventHandler? Loaded;

    public Controller()
    {
        Paths = [];
        _robots = [];
        _walls = [];
        _packages = [];

        _board = new(0, 0);
        _interval = TimeSpan.FromSeconds(1);
        Timer = new Timer(OnTick, null, _interval, _interval); // this is probably better then System.Timers.Timer (it is already asynchronous)
    }

    protected abstract void OnTick(object? state);

    protected void OnLoaded(object? sender)
    {
        Loaded?.Invoke(sender, EventArgs.Empty);
    }

    protected void CallTick(object? sender)
    {
        Tick?.Invoke(sender, EventArgs.Empty);
    }

    public void ChangeSpeed(double speed)
    {
        if (speed <= 0)
        {
            throw new ArgumentException("Speed must be positive", nameof(speed));
        }
        Timer.Change(TimeSpan.FromSeconds(1), _interval / speed);
    }
}