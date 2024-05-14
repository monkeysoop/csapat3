namespace Mekkdonalds.ViewModel;

/// <summary>
/// ViewModel for the replay view using an instance of <see cref="ReplayController"/>."/>
/// </summary>
internal class ReplayViewModel : ViewModel
{
    private readonly ReplayController RepController;

    #region Properties

    /// <summary>
    /// Current time in the replay.
    /// </summary>
    public int CurrentTime
    {
        get => RepController.TimeStamp;
        set
        {
            if (CurrentTime != value)
            {
                RepController.JumpTo(value);
                OnPropertyChanged(nameof(CurrentTime));
                OnPropertyChanged(nameof(TimeLabel));
            }
        }
    }

    /// <summary>
    /// Length of the replay.
    /// </summary>
    public int ReplayLength => RepController.Length;

    /// <summary>
    /// Bindable string of the current time in the replay.
    /// </summary>
    public string TimeLabel
    {
        get => CurrentTime.ToString();
        set
        {
            if (int.TryParse(value, out var time))
            {
                if (time < 0)
                {
                    CurrentTime = 0;
                }
                else if (time > ReplayLength)
                {
                    CurrentTime = ReplayLength;
                }
                else if (time != CurrentTime)
                {
                    CurrentTime = time;
                }
            }
            else
            {
                OnPropertyChanged(nameof(TimeLabel));
            }
        }
    }

    /// <summary>
    /// Formatted string of the length of the replay.
    /// </summary>
    public string LengthLabel => $"/{ReplayLength}";

    #endregion

    #region Commands

    /// <summary>
    /// Command to step backward in the replay.
    /// </summary>
    public ICommand Backward { get; }

    #endregion

    /// <summary>
    /// Creates a new instance of <see cref="ReplayViewModel"/>.
    /// </summary>
    /// <param name="logPath"></param>
    /// <param name="mapPath"></param>
    /// <exception cref="ArgumentException"></exception>
    public ReplayViewModel(string logPath, string mapPath) : base(new ReplayController(logPath, mapPath, ReplayDataAccess.Instance))
    {
        if (Controller is not ReplayController controller)
        {
            throw new ArgumentException("Controller is not a ReplayController");
        }
        else
        {
            RepController = controller;
        }

        RepController.Tick += OnTick;
        RepController.Loaded += OnLoaded;

        Backward = new DelegateCommand(_ => RepController.StepBackward());
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(CurrentTime));
        OnPropertyChanged(nameof(ReplayLength));
        OnPropertyChanged(nameof(LengthLabel));
        OnPropertyChanged(nameof(TimeLabel));

        OnLoaded(this);
    }

    private void OnTick(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(CurrentTime));
        OnPropertyChanged(nameof(TimeLabel));

        OnTick(this);
    }
}
