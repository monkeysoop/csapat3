namespace Mekkdonalds.ViewModel;

internal class ReplayViewModel : ViewModel
{
    private readonly ReplayController RepController;

    #region Properties

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

    public int ReplayLength => RepController.Length;

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

    public string LengthLabel => $"/{ReplayLength}";

    #endregion

    #region Commands

    public ICommand Backward { get; }

    #endregion

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
