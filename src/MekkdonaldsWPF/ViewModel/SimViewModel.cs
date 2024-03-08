namespace Mekkdonalds.ViewModel;

internal abstract class SimViewModel : ViewModelBase
{
    private int _size;
    public int Size
    {
        get => _size;
        set
        {
            if (_size != value)
            {
                _size = value;
                OnPropertyChanged(nameof(Size));
            }
        }
    }
}
