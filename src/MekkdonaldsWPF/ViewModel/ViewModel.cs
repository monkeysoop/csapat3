namespace Mekkdonalds.ViewModel;

internal abstract class ViewModel : ViewModelBase
{
#pragma warning disable CS8618 // :)
    protected Controller Controller;
#pragma warning restore CS8618

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
                if (value < 0.1)
                    value = 0.1;
                else if (value > 8)
                    value = 8;
                _zoom = value;
                OnPropertyChanged(nameof(Zoom));
                OnPropertyChanged(nameof(ZoomLabel));
            }
        }
    }

    public string ZoomLabel => $"{Zoom:0.##}x";

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

    #endregion

    /// <summary>
    /// Eventhandler thats called each time the grid gets updated
    /// </summary>
    public event EventHandler? Tick;
    public event EventHandler? Loaded;

    protected void OnLoaded(object? sender)
    {
        Play = new DelegateCommand(_ => Controller.Play());
        Pause = new DelegateCommand(_ => Controller.Pause());
        Forward = new DelegateCommand(_ => Controller.StepForward());
        OnPropertyChanged(nameof(Play));
        OnPropertyChanged(nameof(Pause));
        OnPropertyChanged(nameof(Forward));
        Loaded?.Invoke(sender, EventArgs.Empty);
    }

    /// <summary>
    /// Calls the tick event based on the models evnt handler
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
