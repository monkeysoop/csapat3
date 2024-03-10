namespace Mekkdonalds.Simulation.Controller;

internal sealed class DFSController : SimulationController
{
    public DFSController(List<Robot> r) : base(r)
    {
    }

    protected override Task CalculatePath(Robot robot)
    {
        throw new NotImplementedException();
    }
}
