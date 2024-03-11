namespace Mekkdonalds.Simulation.Controller;

public sealed class DFSController : SimulationController
{
    public DFSController() : this(1) { }

    public DFSController(double interval) : base(interval) { }


    protected override Task CalculatePath(Robot robot)
    {
        Paths[robot] = new Path();

        return Task.CompletedTask;
    }
}
