namespace Mekkdonalds.Simulation.Controller;

internal abstract class SimulationController : Controller
{
    protected SimulationController(List<Robot> r, double interval) : base(r, interval)
    {
        var tasks = new List<Task>();
        Robots.ForEach(x => tasks.Add(CalculatePath(x)));

        Task.WaitAll([.. tasks]);
    }

    protected abstract Task CalculatePath(Robot robot);
    
    protected override void OnTick(object? state)
    {
        throw new NotImplementedException();
    }
}
