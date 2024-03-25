using Mekkdonalds.Persistence;

namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    private readonly SimulationController SimController;

    public SimulationViewModel(string path) : base()
    {
        Controller = SimController = new(path, new ConfigDataAccess(), new BoardFileDataAccess(), new RobotsDataAccess(), new PackagesDataAccess());

        SimController.Loaded += (_, _) => OnLoaded(this);
        SimController.Tick += (_, _) => OnTick(this);
    }
}
