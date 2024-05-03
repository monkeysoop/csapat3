using Mekkdonalds.Simulation.Assigner;

namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    private readonly SimulationController SimulationController;

    public ICommand LogSave { get; }

    public SimulationViewModel(string path, Type pathfinder) : base(new SimulationController(path, SimDataAccess.Instance, typeof(RoundRobinAssigner), pathfinder))
    {
        if (Controller is not SimulationController controller)
        {
            throw new ArgumentException("Controller is not a SimulationController");
        }
        else
        {
            SimulationController = controller;
        }

        SimulationController.Loaded += (_, _) => OnLoaded(this);
        SimulationController.Tick += (_, _) => OnTick(this);

        LogSave = new DelegateCommand(_ => SimulationController.SaveLog());
    }

    internal void AssignTask(Robot selectedRobot, int x, int y)
    {
        SimulationController.Assign(selectedRobot, new(x, y));
    }
}
