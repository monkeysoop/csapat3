using Mekkdonalds.Simulation.Controller;

namespace Mekkdonalds.ViewModel;

internal class ReplayViewModel : ViewModel
{
    private readonly ReplayController RepController;
    private int _currentTime;

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

    #region Commands

    public readonly ICommand Backward;

    #endregion

    public ReplayViewModel(string logPath)
    {
        Controller = RepController = new ReplayController(logPath, "");

        Controller.Tick += OnTick;

        Zoom = 2;

        Backward = new DelegateCommand(_ => RepController.StepBackward());
    }

    private void OnTick(object? sender, EventArgs e)
    {
        OnPropertyChanged(nameof(TimeLabel));
        OnTick(this);
    }
}
