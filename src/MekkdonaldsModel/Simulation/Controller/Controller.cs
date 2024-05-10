namespace Mekkdonalds.Simulation.Controller;
/// <summary>
/// Abstract controller for the simulation
/// </summary>
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
    /// Speed of the simulation
    /// </summary>
    public double Speed { get; private set; } = 1;

    /// <summary>
    /// Width of the map
    /// </summary>
    public int Width => _board.Width;
    /// <summary>
    /// Height of the map
    /// </summary>
    public int Height => _board.Height;
    /// <summary>
    /// Which step the simulation is at
    /// </summary>
    public int TimeStamp { get; protected set; }

    /// <summary>
    /// Readonly list of robots
    /// </summary>
    public IReadOnlyList<Robot> Robots => _robots.AsReadOnly();

    /// <summary>
    /// Readonly list of walls
    /// </summary>
    public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

    /// <summary>
    /// The event that occurs when an exception is thrown
    /// </summary>
    public event EventHandler<System.Exception>? Exception;

    /// <summary>
    /// The event that occurs on each tick
    /// </summary>
    public event EventHandler? Tick;

    /// <summary>
    /// Event that occurs when the controller is loaded
    /// </summary>
    public event EventHandler? Loaded;


    /// <summary>
    /// Constructor for the controller
    /// </summary>
    /// <param name="speed">Speed of the simulation</param> 
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
    /// Default constructor for the controller
    /// </summary>
    protected Controller() : this(.2) { }

    /// <summary>
    /// Method to be called on each tick
    /// </summary>
    /// <param name="state">A state</param> 
    protected abstract void OnTick(object? state);


    /// <summary>
    /// Method to be called when the controller is loaded
    /// </summary>
    /// <param name="sender">The object that sent the event</param> 
    protected void OnLoaded(object? sender)
    {
        Loaded?.Invoke(sender, EventArgs.Empty);
        Timer.Change(TimeSpan.Zero, Interval);
        IsPlaying = true;
    }

    /// <summary>
    /// Method to be called when the controller is ticked
    /// </summary>
    /// <param name="sender">The object that sent the event</param> 
    protected void CallTick(object? sender)
    {
        Tick?.Invoke(sender, EventArgs.Empty);
    }


    /// <summary>
    /// Loads the walls
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
    /// Method to be called when an exception is thrown
    /// </summary>
    /// <param name="sender">The object that sent the event</param> 
    /// <param name="e">The exception that was thrown</param> 
    protected void OnException(object? sender, System.Exception e)
    {
        Exception?.Invoke(sender, e);
    }

    /// <summary>
    /// Changes the speed of the simulation
    /// </summary>
    /// <param name="speed">The speed of the simulation</param> 
    /// <exception cref="ArgumentException">To be thrown when the speed is not positive</exception> 
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
    /// Plays the simulation
    /// </summary>
    public void Play()
    {
        Timer.Change(TimeSpan.Zero, Interval);
        IsPlaying = true;
    }

    /// <summary>
    /// Pauses the simulation
    /// </summary>
    public void Pause()
    {
        Timer.Change(Timeout.Infinite, Timeout.Infinite);
        IsPlaying = false;
    }

    /// <summary>
    /// Abstract method to step forward in the simulation
    /// </summary>
    public abstract void StepForward();
}
