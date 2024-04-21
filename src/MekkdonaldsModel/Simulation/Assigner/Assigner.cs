using System.Diagnostics.CodeAnalysis;

namespace Mekkdonalds.Simulation.Assigner;

internal class Assigner : IAssigner
{
    private readonly ConcurrentQueue<Package> _packages = [];
    private readonly ConcurrentDictionary<Robot, Path> _paths = [];
    private readonly List<Robot> _robots = [];
    private Board _board = new(0, 0);
    private PathFinder _pathFinder = new Astar();

    private int cost_counter = 0;
    public int TimeStamp { get { return cost_counter; } }

    public event EventHandler? Ended;

    [NotNull]
    private Logger? _logger;

    public void Init(ControllerType algorithm, Board board, IEnumerable<Robot> robots, IEnumerable<Package> packages, Logger logger)
    {
        _logger = logger;

        _board = board;

        foreach (Package package in packages)
        {
            _packages.Enqueue(package);
        }

        _pathFinder = algorithm switch
        {
            ControllerType.Astar => new Astar(),
            ControllerType.BFS => new BFS(),
            ControllerType.DFS => new DFS(),
            _ => throw new NotImplementedException()
        };

        _robots.AddRange(robots);

        foreach (Robot robot in _robots)
        {
            Assign(robot);
        }
    }

    public void Step()
    {
        lock (_board)
        {
            if (_packages.IsEmpty && _paths.IsEmpty)
            {
                Ended?.Invoke(this, EventArgs.Empty);
                return;
            }

            foreach (Robot robot in _robots)
            {
                Path? path;
                if (!_paths.TryGetValue(robot, out path))
                {
                    // sometimes this is caused by pathfinding taking too long and it times out
                    throw new System.Exception("");
                }
                if (path == null)
                {
                    throw new System.Exception("");
                }

                if (path.IsOver)
                {
                    _logger.LogPlannerPaths(robot.ID, path);
                    _logger.LogFinish(robot.ID, robot.Task!.ID, TimeStamp);
                    
                    if (!_paths.TryRemove(robot, out var _))
                    {
                        throw new System.Exception("");
                    }
                    
                    path = Assign(robot);
                }

                if (!path.IsOver)
                {
                    Action action = path.PeekNext();

                    if (robot.TryStep(action, _board, cost_counter))
                    {
                        path.Increment();
                    }
                    else
                    {
                        Free(robot, path);
                        _board.Reserve(robot.Position, cost_counter + 1);
                    }
                }
                else
                {
                    // this could happen if the next task is at the same position as the robot which is assigned to
                    _board.UnReserve(robot.Position, cost_counter);
                }

                //if (_paths.TryGetValue(r, out var path))
                //{
                //    if (path.IsOver)
                //    {
                //        _logger.LogPlannerPaths(r.ID, path);
                //        _logger.LogFinish(r.ID, r.Task!.ID, TimeStamp); // Task is not null here (hopefully)
                //
                //        r.AddTask((Package?)null);
                //        if (_paths.TryRemove(r, out _))
                //            Assign(r);
                //
                //
                //        if (!_paths.TryGetValue(r, out path))
                //        {
                //            continue;
                //        }
                //
                //        if (path.IsOver)
                //        {
                //            r.Step(Action.W);
                //            continue;
                //        }
                //    }
                //
                //    var action = path.Next();
                //
                //    if (action is Action.F)
                //    {
                //        // collision detection
                //    }
                //
                //    r.Step(action);
                //
                //}
            }

            cost_counter++;
        }

    }

    private Path Assign(Robot robot)
    {
        Package? task = null;
        Point? task_pos = null;
        bool found = false;
        List<Action> actions = [Action.W];

        Package? package = null;
        if (_packages.TryPeek(out package))
        {
            //Debug.Write(robot.ID + ": ");
            (found, actions) = _pathFinder.CalculatePath(_board, robot.Position, (int)robot.Direction, package.Position, cost_counter);

            if (found)
            {
                //Debug.WriteLine(robot.ID);
                //foreach(Action action in actions)
                //{
                //    Debug.Write(action.ToString());
                //}
                //Debug.WriteLine("");
                task = package;
                task_pos = package.Position;
                _packages.TryDequeue(out _);
                _logger.LogAssignment(robot.ID, package.ID, TimeStamp);
            }
            else
            {
                _board.Reserve(robot.Position, cost_counter + 1);
            }

        }
        else
        {
            // no package
            _board.Reserve(robot.Position, cost_counter + 1);
        }


        robot.AddTask(task);
        Path path = new Path(actions, task_pos);
        if (!_paths.TryAdd(robot, path))
        {
            throw new System.Exception("");
        }
        
        return path;
    }
    //private void Assign(Robot r)
    //{
    //    if (_packages.TryDequeue(out var p))
    //    {
    //        var (found, path) = _pathFinder.CalculatePath(_board, r.Position, (int)r.Direction, p.Position);
    //
    //        if (found)
    //        {
    //            if (_paths.TryAdd(r, new(path, p.Position)))
    //            {
    //                r.AddTask(p);
    //                _logger.LogAssignment(r.ID, p.ID, TimeStamp);
    //            }
    //        }
    //    }
    //}

    private void Free(Robot robot, Path path)
    {
        if (!path.FreeAllReserved(_board, robot.Position, robot.Direction, cost_counter))
        {
            throw new System.Exception("");
        }
        if (!_paths.TryRemove(robot, out var _))
        {
            throw new System.Exception("");
        }
        if (!_paths.TryAdd(robot, new Path(new List<Action>(), null)))
        {
            throw new System.Exception("");
        }
        Package package = robot.RemoveTask();
        _packages.Enqueue(package);
    }
}
