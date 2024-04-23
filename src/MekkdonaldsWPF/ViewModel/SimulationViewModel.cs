using Mekkdonalds.Simulation.Assigner;

namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    private readonly SimulationController SimulationController;

    public ICommand LogSave { get; }

    public SimulationViewModel(string path, ControllerType algorithm) : base()
    {
        Controller = SimulationController = new(path, SimDataAccess.Instance, algorithm, typeof(RoundRobinAssigner));

        SimulationController.Loaded += (_, _) => OnLoaded(this);
        SimulationController.Tick += (_, _) => OnTick(this);

        LogSave = new DelegateCommand(_ => SimulationController.SaveLog());
    }
}
