namespace Mekkdonalds.Simulation.Controller;

internal abstract class Controller
{
    protected Dictionary<Robot, Path> Paths;
    protected List<Robot> Robots;

    public Controller(List<Robot> r)
    {
        Paths = [];
        Robots = [];
        Robots.AddRange(r);
    }
}