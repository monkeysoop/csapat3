using Mekkdonalds.Simulation.Controller;

namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    private SimulationController Controller;

    public SimulationViewModel() : base()
    {
        Size = (20, 40);

        Controller = new DFSController();

        _walls.AddRange(Controller.Walls);
        _robots.AddRange(Controller.Robots);

        Controller.Tick += (_, _) => OnTick(this);
    }
}
