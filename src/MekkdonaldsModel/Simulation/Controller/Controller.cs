namespace Mekkdonalds.Simulation.Controller;

public abstract class Controller
{
    protected List<Robot> _robots;
    protected List<Wall> _walls;
    protected Timer Timer;

    private readonly TimeSpan _interval;
    /// <summary>
    /// Represents the state of the controller, <see langword="true"/> if the action controlled by controller is over
    /// </summary>
    public bool IsOver { get; protected set; }

    protected TimeSpan Interval => _interval / Speed;

    protected Board _board;

    /// <summary>
    /// Indicates the state of the controller, <see langword="true"/> if the controller is playing, <see langword="false"/> if it is paused.
    /// </summary>
    public bool IsPlaying { get; protected set; }
    /// <summary>
    /// Speed of the controller, the time between each tick in seconds.
    /// </summary>
    public double Speed { get; private set; } = 1;

    /// <summary>
    /// Width of the map.
    /// </summary>
    public int Width => _board.Width;
    /// <summary>
    /// Height of the map.
    /// </summary>
    public int Height => _board.Height;

    /// <summary>
    /// List of robots in the controlled by the controller.
    /// </summary>
    public IReadOnlyList<Robot> Robots => _robots.AsReadOnly();
    /// <summary>
    /// List of walls on the map.
    /// </summary>
    public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

    /// <summary>
    /// Occurs when the controller encounters an exception.
    /// </summary>
    public event EventHandler<System.Exception>? Exception;

    /// <summary>
    /// Occurs when the controller ticks.
    /// </summary>
    public event EventHandler? Tick;
    /// <summary>
    /// Occurs when the controller is loaded.
    /// </summary>
    public event EventHandler? Loaded;

    protected Controller(double speed)
    {
        _robots = [];
        _walls = [];

        _board = new(0, 0);
        _interval = TimeSpan.FromSeconds(speed);

        Timer = new Timer((e) =>
        {
            try
            {
                OnTick(e);
            }
            catch (System.Exception ex)
            {
                OnException(this, ex);
            }
        }, null, Timeout.Infinite, Timeout.Infinite);
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

    protected void OnException(object? sender, System.Exception e)
    {
        Exception?.Invoke(sender, e);
    }

    /// <summary>
    /// Sets the speed of the controller.
    /// </summary>
    /// <param name="speed">Multiple of the current speed</param>
    /// <exception cref="ArgumentException">Gets thrown when the speed is non-positive</exception>
    public void ChangeSpeed(double speed)
    {
        if (speed <= 0)
        {
            throw new ArgumentException("Speed must be positive", nameof(speed));
        }

        Speed = speed;

        if (IsPlaying) Timer.Change(TimeSpan.Zero, Interval);
    }

    /// <summary>
    /// Resumes the controller.
    /// </summary>
    public virtual void Play()
    {
        if (!IsPlaying)
        {
            Timer.Change(TimeSpan.Zero, Interval);
            IsPlaying = true;
        }
    }

    /// <summary>
    /// Stops the controller.
    /// </summary>
    public virtual void Pause()
    {
        if (IsPlaying)
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            IsPlaying = false;
        }
    }

    /// <summary>
    /// Steps the controller forward by one step.
    /// </summary>
    public abstract void StepForward();
}
