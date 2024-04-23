namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    private readonly SimulationController SimController;

    public SimulationViewModel(string path, ControllerType algorithm) : base()
    {
        Controller = SimController = new(path, SimDataAccess.Instance, algorithm);

        SimController.Loaded += (_, _) => OnLoaded(this);
        SimController.Tick += (_, _) => OnTick(this);
    }
}
