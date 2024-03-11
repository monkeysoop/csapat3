namespace Mekkdonalds.Simulation.Controller;

internal sealed class DFSController : SimulationController
{
    public DFSController(List<Robot> r) : this(r, 1) { }

    public DFSController(List<Robot> r, double interval) : base(r, interval) { }


    protected override Task CalculatePath(Robot robot)
    {
        throw new NotImplementedException();
    }
}
