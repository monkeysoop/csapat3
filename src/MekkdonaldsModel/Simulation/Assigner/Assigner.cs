namespace Mekkdonalds.Simulation.Assigner;

public abstract class Assigner
{
    protected readonly IEnumerable<Package> _packages;
    protected Board _board;

    protected Assigner(ControllerType type, Board board, IEnumerable<Package> packages, IEnumerable<Robot> robots)
    {
        _board = board;
        _packages = new ConcurrentQueue<Package>(packages);
        TimeStamp = 0;
    }

    public int TimeStamp { get; }
    public event EventHandler? Ended;
    public abstract void Step();

    protected void CallEnded(object? caller, EventArgs e) => Ended?.Invoke(caller, e);
}
