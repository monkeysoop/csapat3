namespace Mekkdonalds.Simulation.Controller;

internal abstract class Controller
{
    protected Dictionary<Robot, Path> Paths;
    protected List<Robot> Robots;
    protected Timer Timer;

    public Controller(List<Robot> r, double interval)
    {
        Paths = [];
        Robots = [];
        Robots.AddRange(r);
        Timer = new Timer(OnTick, null, TimeSpan.Zero, TimeSpan.FromSeconds(interval)); // this is probably better then System.Timers.Timer (it is already asynchronous)
    }

    protected abstract void OnTick(object? state);
}