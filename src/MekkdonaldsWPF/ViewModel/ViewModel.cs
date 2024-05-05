namespace Mekkdonalds.ViewModel;

internal abstract class ViewModel : ViewModelBase
{
    private const double MINZOOM = .3;
    private const double MAXZOOM = 2;

    protected Controller Controller;

    private double _zoom = 1;

    #region Properties

    public int Width => Controller.Width;
    public int Height => Controller.Height;

    public double Zoom
    {
        get => _zoom;
        set
        {
            if (_zoom != value)
            {
                if (value < MINZOOM || value > MAXZOOM)
                    return;

                _zoom = value;
                OnPropertyChanged(nameof(Zoom));
                OnPropertyChanged(nameof(SpeedLabel));
            }
        }
    }

    public string SpeedLabel => $"{Controller.Speed:##.##}x";

    /// <summary>
    /// Robots present on the grid
    /// </summary>
    public IReadOnlyList<Robot> Robots => Controller.Robots;
    /// <summary>
    /// Walls present on the grid
    /// </summary>
    public IReadOnlyList<Wall> Walls => Controller.Walls;

    public ICommand Play { get; private set; } = new DelegateCommand(_ => { });
    public ICommand Pause { get; private set; } = new DelegateCommand(_ => { });
    public ICommand Forward { get; private set; } = new DelegateCommand(_ => { });

    public ICommand SpeedDown { get; private set; } = new DelegateCommand(_ => { });
    public ICommand SpeedUp { get; private set; } = new DelegateCommand(_ => { });

    #endregion

    /// <summary>
    /// Event handler that's called each time the grid gets updated
    /// </summary>
    public event EventHandler? Tick;
    public event EventHandler? Loaded;
    public event EventHandler<System.Exception>? Exception;

    protected ViewModel(Controller controller)
    {
        Controller = controller;
        Controller.Exception += OnException;
    }

    private void OnException(object? sender, System.Exception e) => Exception?.Invoke(sender, e);

    protected void OnLoaded(object? sender)
    {
        Play = new DelegateCommand(_ => Controller.Play());
        Pause = new DelegateCommand(_ => Controller.Pause());
        Forward = new DelegateCommand(_ => Controller.StepForward());
        SpeedDown = new DelegateCommand(_ => ChangeSpeed(Controller.Speed * .8));
        SpeedUp = new DelegateCommand(_ => ChangeSpeed(Controller.Speed * 1.25));

        OnPropertyChanged(nameof(Play));
        OnPropertyChanged(nameof(Pause));
        OnPropertyChanged(nameof(Forward));
        OnPropertyChanged(nameof(SpeedDown));
        OnPropertyChanged(nameof(SpeedUp));

        Loaded?.Invoke(sender, EventArgs.Empty);
    }

    private void ChangeSpeed(double speed)
    {
        Controller.ChangeSpeed(speed);
        OnPropertyChanged(nameof(SpeedLabel));
    }

    /// <summary>
    /// Calls the tick event based on the models event handler
    /// </summary>
    /// <param name="sender"></param>
    protected void OnTick(object? sender)
    {
        Tick?.Invoke(sender, EventArgs.Empty);
    }

    internal void Toggle()
    {
        if (Controller.IsPlaying)
            Controller.Pause();
        else
            Controller.Play();
    }
}
