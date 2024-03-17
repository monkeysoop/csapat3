namespace Mekkdonalds.ViewModel;

internal class SimulationViewModel : ViewModel
{
    private readonly SimulationController Controller;

    public SimulationViewModel(string path) : base()
    {
        Size = (20, 40);

        Controller = new DFSController(path);

        _walls.AddRange(Controller.Walls);
        _robots.AddRange(Controller.Robots);

        Controller.Tick += (_, _) => OnTick(this);
    }
}
