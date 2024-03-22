namespace Mekkdonalds.ViewModel;

internal abstract class ViewModel() : ViewModelBase
{
    protected List<Robot> _robots = [];
    protected List<Wall> _walls = [];

    private (int W, int H) _size;
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

    public IReadOnlyList<Robot> Robots => _robots.AsReadOnly();
    public IReadOnlyList<Wall> Walls => _walls.AsReadOnly();

    public event EventHandler? Tick;

    protected void OnTick(object? sender)
    {
        Tick?.Invoke(sender, EventArgs.Empty);
    }
}
