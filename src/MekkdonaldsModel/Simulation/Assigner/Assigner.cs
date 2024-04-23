namespace Mekkdonalds.Simulation.Assigner;

public abstract class Assigner(ControllerType type, Board board, IEnumerable<Package> packages, IEnumerable<Robot> robots)
{
    protected readonly IEnumerable<Package> _packages = packages;
    protected readonly IEnumerable<Robot> _robots = robots;
    protected Board _board = board;

    public virtual bool NoPackage { get; }
    public int TimeStamp { get; } = 0;
    public event EventHandler? Ended;

    public abstract bool Peek(out Package? package);
    public abstract void Get(out Package? package);

    public virtual void Return(Package package) { }

    protected void CallEnded(object? caller, EventArgs e) => Ended?.Invoke(caller, e);
}
