using Mekkdonalds.Simulation.Controller;

namespace Mekkdonalds.ViewModel;

internal class ReplayViewModel : ViewModel
{
    private readonly ReplayController Controller;

    public ReplayViewModel()
    {
        Size = (20, 40);

        Controller = new ReplayController();

        _walls.AddRange(Controller.Walls);
        _robots.AddRange(Controller.Robots);

        Controller.Tick += (_, _) => OnTick(this);
    }
}
