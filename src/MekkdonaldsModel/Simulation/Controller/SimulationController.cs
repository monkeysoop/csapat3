
namespace Mekkdonalds.Simulation.Controller;

public abstract class SimulationController : Controller
{
    public int Cost { get; protected set; } // Apperently 32bit value types are atomic in c# by default
    private TimeSpan Elapsed;

    protected SimulationController(double interval) : base()
    {
        var tasks = new List<Task>();

        _robots.AddRange([new(1, 0, 0), new(2, 10, 25), new(3, 2, 2)]);
        _walls.AddRange([new(1, 1), new(1, 3)]);

        _robots.ForEach(x => tasks.Add(CalculatePath(x)));

        Task.WaitAll([.. tasks]);

        Timer.Change(TimeSpan.FromSeconds(1), new TimeSpan(0, 0, 1));
    }

    protected abstract Task CalculatePath(Robot robot);
    
    protected override void OnTick(object? state)
    {
        Task.Run(() => { _robots.ForEach(r => r.Step(Paths[r].Next())); });
        Elapsed += new TimeSpan(0, 0, 1);

        CallTick(this);
    }
}
