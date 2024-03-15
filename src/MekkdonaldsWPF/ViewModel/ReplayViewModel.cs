namespace Mekkdonalds.ViewModel;

internal class ReplayViewModel : ViewModel
{
    private readonly ReplayController Controller;
    private int _currentTime;
    private int _replayLength;

    #region Properties

    public int CurrentTime
    {
        get => _currentTime;
        set
        {
            _currentTime = value;
            OnPropertyChanged(nameof(CurrentTime));
        }
    }

    public int ReplayLength
    {
        get => _replayLength;
        private set
        {
            _replayLength = value;
            OnPropertyChanged(nameof(ReplayLength));
        }
    }

    #endregion

    public ReplayViewModel(string logPath)
    {
        Size = (20, 40);

        Controller = new ReplayController(logPath);

        _walls.AddRange(Controller.Walls);
        _robots.AddRange(Controller.Robots);

        Controller.Tick += (_, _) => OnTick(this);
    }
}
