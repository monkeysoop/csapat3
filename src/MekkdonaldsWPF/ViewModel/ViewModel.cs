namespace Mekkdonalds.ViewModel;

/// <summary>
/// Abstract base class for viewmodels
/// </summary>
internal abstract class ViewModel : ViewModelBase
{
    protected List<Robot> _robots = [];
    protected List<Wall> _walls = [];

    private (int W, int H) _size;
    private double _zoom = 1;

    #region Properties

    /// <summary>
    /// Size of the grid (Collumns, Rows)
    /// </summary>
    public (int W, int H) Size
    {
        get => _size;
        protected set
        {
            if (_size != value)
            {
                _size = value;
                OnPropertyChanged(nameof(Size));
            }
        }
    }

    public double Zoom
    {
        get => _zoom;
        set
        {
            if (_zoom != value)
            {
                _zoom = value;
                OnPropertyChanged(nameof(Zoom));
            }
        }
    }

    /// <summary>
    /// Robots present on the grid
    /// </summary>
    public IReadOnlyList<Robot> Robots => _robots.AsReadOnly();
    /// <summary>
    /// Walls present on the grid
    /// </summary>
    public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

    #endregion

    /// <summary>
    /// Eventhandler thats called each time the grid gets updated
    /// </summary>
    public event EventHandler? Tick;
    /// <summary>
    /// Calls the tick event based on the models evnt handler
    /// </summary>
    /// <param name="sender"></param>
    protected void OnTick(object? sender)
    {
        Tick?.Invoke(sender, EventArgs.Empty);
    }
}
