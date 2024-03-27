namespace Mekkdonalds.Simulation.PathFinding;

public interface IPathFinder
{
    void Init(Board2 board, IEnumerable<Package> packages);
    void Assign(Robot r);
    void Step(Robot r);
}
