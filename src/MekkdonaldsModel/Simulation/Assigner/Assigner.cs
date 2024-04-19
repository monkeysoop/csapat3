using System.Diagnostics.CodeAnalysis;

namespace Mekkdonalds.Simulation.Assigner;

internal class Assigner : IAssigner
{
    private readonly ConcurrentQueue<Package> _packages = [];
    private readonly ConcurrentDictionary<Robot, Path> Paths = [];
    private readonly List<Robot> _robots = [];
    private Board _board = new(0, 0);
    private PathFinder _pathFinder = new DFS();

    public int TimeStamp { get; private set; }

    public event EventHandler? Ended;

    [NotNull]
    private Logger _logger;

    public void Init(ControllerType type, Board board, IEnumerable<Robot> robots, IEnumerable<Package> packages, Logger logger)
    {
        _logger = logger;

        _board = board;

        foreach (var p in packages)
        {
            _packages.Enqueue(p);
        }

        _pathFinder = type switch
        {
            ControllerType.Astar => new Astar(),
            ControllerType.BFS => new BFS(),
            ControllerType.DFS => new DFS(),
            _ => throw new NotImplementedException()
        };

        _robots.AddRange(robots);

        foreach (var r in _robots)
        {
            Assign(r);
        }
    }

    public void Step()
    {
        if (_packages.IsEmpty && Paths.IsEmpty)
        {
            Ended?.Invoke(this, EventArgs.Empty);
            return;
        }

        lock (_board)
        {
            foreach (var r in _robots)
            {
                if (Paths.TryGetValue(r, out var path))
                {
                    if (path.IsOver)
                    {
                        _logger.LogPlannerPaths(r.ID, path);
                        _logger.LogFinish(r.ID, r.Task!.ID, TimeStamp); // Task is not null here (hopefully)

                        r.AddTask((Package?)null);
                        if (Paths.TryRemove(r, out _))
                            Assign(r);
                    }
                    else
                    {
                        var action = path.Next();

                        if (action is Action.F)
                        {
                            // collision detection
                        }

                        r.Step(action);
                    }
                }
            }
        }

        TimeStamp++;
    }

    private void Assign(Robot r)
    {
        if (_packages.TryDequeue(out var p))
        {
            var (found, path) = _pathFinder.CalculatePath(_board, r.Position, (int)r.Direction, p.Position);

            if (found)
            {
                if (Paths.TryAdd(r, new(path, p.Position)))
                {
                    r.AddTask(p);
                    _logger.LogAssignment(r.ID, p.ID, TimeStamp);
                }
            }
        }
    }
}
