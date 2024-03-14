using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Mekkdonalds.ViewModel;

/// <summary>
/// Abstract base class for generic viewmodels
/// </summary>
public abstract class ViewModelBase : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}