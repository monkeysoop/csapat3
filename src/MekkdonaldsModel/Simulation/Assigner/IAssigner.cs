namespace Mekkdonalds.Simulation.Assigner;

public interface IAssigner
{
    void Init(ControllerType type, Board board, IEnumerable<Robot> robots, IEnumerable<Package> packages);
    void Step();
}
