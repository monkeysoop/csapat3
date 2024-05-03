using Mekkdonalds.Simulation.Assigner;

namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    private readonly SimulationController SimulationController;

    public ICommand LogSave { get; }

    public SimulationViewModel(string path, Type pathfinder) : base()
    {
        Controller = SimulationController = new(path, SimDataAccess.Instance, typeof(RoundRobinAssigner), pathfinder);

        SimulationController.Loaded += (_, _) => OnLoaded(this);
        SimulationController.Tick += (_, _) => OnTick(this);

        LogSave = new DelegateCommand(_ => SimulationController.SaveLog());
    }

    internal void AssignTask(Robot selectedRobot, int x, int y)
    {
        SimulationController.Assign(selectedRobot, new(x, y));
    }
}
