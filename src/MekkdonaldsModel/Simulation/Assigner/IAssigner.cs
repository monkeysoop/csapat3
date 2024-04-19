namespace Mekkdonalds.Simulation.Assigner;

public interface IAssigner
{
    int TimeStamp { get; }
    event EventHandler? Ended;
    void Init(ControllerType type, Board board, IEnumerable<Robot> robots, IEnumerable<Package> packages, Logger logger);
    void Step();
}
