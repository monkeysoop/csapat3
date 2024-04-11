using Mekkdonalds.Persistence;
using Mekkdonalds.Simulation.Controller;

namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    private readonly SimulationController SimController;

    public SimulationViewModel(string path) : base()
    {
        var da = new SimDataAccess
        {
            CDA = new ConfigDataAccess(),
            BDA = new BoardFileDataAccess(),
            RDA = new RobotsDataAccess(),
            PDA = new PackagesDataAccess()
        };

        Controller = SimController = new(path, da);

        SimController.Loaded += (_, _) => OnLoaded(this);
        SimController.Tick += (_, _) => OnTick(this);
    }
}
