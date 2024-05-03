namespace Mekkdonalds.Simulation.Controller;

public abstract class Controller
{
    protected List<Robot> _robots;
    protected List<Wall> _walls;
    protected Timer Timer;
    protected TimeSpan Interval { get; private set; }

    protected Board _board;

    public bool IsPlaying { get; protected set; }
    public double Speed { get; private set; } = 1;

    public int Width => _board.Width;
    public int Height => _board.Height;

    public IReadOnlyList<Robot> Robots => _robots.AsReadOnly();

    public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

    public event EventHandler? Tick;
    public event EventHandler? Loaded;

    protected Controller(double speed)
    {
        _robots = [];
        _walls = [];

        _board = new(0, 0);
        Interval = TimeSpan.FromSeconds(speed);
        Timer = new Timer(OnTick, null, Timeout.Infinite, Timeout.Infinite);
    }

    protected Controller() : this(.2) { }

    protected abstract void OnTick(object? state);

    protected void OnLoaded(object? sender)
    {
        Loaded?.Invoke(sender, EventArgs.Empty);
        Timer.Change(TimeSpan.Zero, Interval);
        IsPlaying = true;
    }

    protected void CallTick(object? sender)
    {
        Tick?.Invoke(sender, EventArgs.Empty);
    }

    protected void LoadWalls()
    {
        for (int y = 0; y < _board.Height; y++)
        {
            for (int x = 0; x < _board.Width; x++)
            {
                if (_board.GetValue(x, y) is Board.WALL)
                    _walls.Add(new(x, y));
            }
        }
    }

    public void ChangeSpeed(double speed)
    {
        if (speed <= 0)
        {
            throw new ArgumentException("Speed must be positive", nameof(speed));
        }
        Timer.Change(TimeSpan.Zero, Interval / speed);
        Speed = speed;
    }

    public void Play()
    {
        Timer.Change(TimeSpan.Zero, Interval);
        IsPlaying = true;
    }

    public void Pause()
    {
        Timer.Change(Timeout.Infinite, Timeout.Infinite);
        IsPlaying = false;
    }

    public abstract void StepForward();
}
