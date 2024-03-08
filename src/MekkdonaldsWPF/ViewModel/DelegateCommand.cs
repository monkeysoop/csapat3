namespace Mekkdonalds.ViewModel;

public class DelegateCommand(Action<object?> execute, Predicate<object?>? canExecute = null) : ICommand
{
    private readonly Action<object?> _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    private Predicate<object?>? _canExecute = canExecute;
    public event EventHandler? CanExecuteChanged;

    public Predicate<object?>? Predicate
    {
        get => _canExecute;
        set
        {
            if (_canExecute == value)
                return;

            _canExecute = value;
            RaiseCanExecuteChanged();
        }
    }

    public bool CanExecute(object? parameter) => _canExecute is null || _canExecute(parameter);

    public void Execute(object? parameter)
    {
        if (!CanExecute(parameter))
            throw new InvalidOperationException("Command execution is disabled");

        _execute(parameter);
    }

    public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
}
