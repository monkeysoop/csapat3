namespace Mekkdonalds.Simulation.Assigner;

internal class Assigner : IAssigner
{
    private readonly ConcurrentQueue<Package> _packages = [];
    private readonly ConcurrentDictionary<Robot, Path> Paths = [];
    private readonly List<Robot> _robots = [];
    private Board _board = new(0, 0);
    private PathFinder _pathFinder = new DFS();

    public void Init(ControllerType type, Board board, IEnumerable<Robot> robots, IEnumerable<Package> packages)
    {
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
        lock (_board)
        {
            foreach (var r in _robots)
            {
                if (Paths.TryGetValue(r, out var path))
                {
                    if (path.IsOver)
                    {
                        r.AddTask(null);
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
    }

    private void Assign(Robot r)
    {
        if (_packages.TryDequeue(out var p))
        {
            var (found, path) = _pathFinder.CalculatePath(_board, r.Position, (int)r.Direction, p.Position);

            if (found)
            {
                if (Paths.TryAdd(r, new(path, p.Position)))
                    r.AddTask(p.Position);
            }
        }
    }
}
