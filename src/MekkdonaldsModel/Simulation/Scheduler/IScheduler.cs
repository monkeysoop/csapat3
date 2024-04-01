namespace Mekkdonalds.Simulation.Scheduler;

public interface IScheduler
{
    void Init(ControllerType type, Board2 board, IEnumerable<Robot> robots, IEnumerable<Package> packages);
    void Step();
}
