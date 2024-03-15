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
            OnPropertyChanged(nameof(TimeLabel));
        }
    }

    public int ReplayLength
    {
        get => _replayLength;
        private set
        {
            _replayLength = value;
            OnPropertyChanged(nameof(ReplayLength));
            OnPropertyChanged(nameof(TimeLabel));
        }
    }

    public string TimeLabel
    {
        get
        {
            var m = CurrentTime / 60;
            var s = CurrentTime % 60;

            var mm = ReplayLength / 60;
            var ms = ReplayLength % 60;

            return $"{(m < 10 ? "0" : "")}{m}:{(s < 10 ? "0" : "")}{s}/{(mm < 10 ? "0" : "")}{mm}:{(ms < 10 ? "0" : "")}{ms}";
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
