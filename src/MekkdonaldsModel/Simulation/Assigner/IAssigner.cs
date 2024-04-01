namespace Mekkdonalds.Simulation.Assigner;

public interface IAssigner
{
    void Init(ControllerType type, Board2 board, IEnumerable<Robot> robots, IEnumerable<Package> packages);
    void Step();
}
