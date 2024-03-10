namespace Mekkdonalds.ViewModel;

internal abstract class ViewModel(List<Robot> r, List<Wall> w) : ViewModelBase
{
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

    public IReadOnlyList<Robot> Robots = r.AsReadOnly();
    public IReadOnlyList<Wall> Walls = w.AsReadOnly();
}
