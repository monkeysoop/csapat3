namespace Mekkdonalds.Simulation.Controller;

public abstract class Controller
{
    protected List<Robot> _robots;
    protected List<Wall> _walls;
    protected Timer Timer;
    protected TimeSpan Interval { get; private set; }

    protected Board _board;

    /// <summary>
    /// Is the simulation playing or not
    /// </summary>
    public bool IsPlaying { get; protected set; }
    /// <summary>
    /// speed of the simulation
    /// </summary>
    public double Speed { get; private set; } = 1;

    /// <summary>
    /// width of the map
    /// </summary>
    public int Width => _board.Width;
    /// <summary>
    /// height of the map
    /// </summary>
    public int Height => _board.Height;
    /// <summary>
    /// which step the simulation is at
    /// </summary>
    public int TimeStamp { get; protected set; }

    /// <summary>
    /// readonly list of robots
    /// </summary>
    public IReadOnlyList<Robot> Robots => _robots.AsReadOnly();

    /// <summary>
    /// readonly list of walls
    /// </summary>
    public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

    /// <summary>
    /// The event to be called when an exception is thrown
    /// </summary>
    public event EventHandler<System.Exception>? Exception;

    /// <summary>
    /// The event to be called on each tick
    /// </summary>
    public event EventHandler? Tick;

    /// <summary>
    /// event to be called when the controller is loaded
    /// </summary>
    public event EventHandler? Loaded;


    /// <summary>
    /// constructor for the controller
    /// </summary>
    /// <param name="speed">speed of the simulation</param> 
    protected Controller(double speed)
    {
        _robots = [];
        _walls = [];

        _board = new(0, 0);
        Interval = TimeSpan.FromSeconds(speed);
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

    /// <summary>
    /// default constructor for the controller
    /// </summary>
    protected Controller() : this(.2) { }

    /// <summary>
    /// method to be called on each tick
    /// </summary>
    /// <param name="state">a state</param> 
    protected abstract void OnTick(object? state);


    /// <summary>
    /// method to be called when the controller is loaded
    /// </summary>
    /// <param name="sender">the object that sent the event</param> 
    protected void OnLoaded(object? sender)
    {
        Loaded?.Invoke(sender, EventArgs.Empty);
        Timer.Change(TimeSpan.Zero, Interval);
        IsPlaying = true;
    }

    /// <summary>
    /// method to be called when the controller is ticked
    /// </summary>
    /// <param name="sender">the object that sent the event</param> 
    protected void CallTick(object? sender)
    {
        Tick?.Invoke(sender, EventArgs.Empty);
    }


    /// <summary>
    /// method to load the walls
    /// </summary>
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

    /// <summary>
    /// method to be called when an exception is thrown
    /// </summary>
    /// <param name="sender">the object that sent the event</param> 
    /// <param name="e">the exception that was thrown</param> 
    protected void OnException(object? sender, System.Exception e)
    {
        Exception?.Invoke(sender, e);
    }

    /// <summary>
    /// method to change the speed of the simulation
    /// </summary>
    /// <param name="speed">the speed of the simulation</param> 
    /// <exception cref="ArgumentException">to be thrown when the speed is not positive</exception> 
    public void ChangeSpeed(double speed)
    {
        if (speed <= 0)
        {
            throw new ArgumentException("Speed must be positive", nameof(speed));
        }
        Timer.Change(TimeSpan.Zero, Interval / speed);
        Speed = speed;
    }

    /// <summary>
    /// method to play the simulation
    /// </summary>
    public void Play()
    {
        Timer.Change(TimeSpan.Zero, Interval);
        IsPlaying = true;
    }

    /// <summary>
    /// method to pause the simulation
    /// </summary>
    public void Pause()
    {
        Timer.Change(Timeout.Infinite, Timeout.Infinite);
        IsPlaying = false;
    }

    /// <summary>
    /// abstract method to step forward in the simulation
    /// </summary>
    public abstract void StepForward();
}
