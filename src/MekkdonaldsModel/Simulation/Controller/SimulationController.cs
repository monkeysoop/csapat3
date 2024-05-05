namespace Mekkdonalds.Simulation.Controller;

public sealed class SimulationController : Controller
{
    private Assigner.Assigner? _assigner;

    private readonly ConcurrentDictionary<Robot, Path> _paths = [];
    private readonly PathFinder _pathFinder = new Astar();

    private Logger _logger;
    private readonly ILogFileDataAccess _logFileDataAccess;

    private readonly double Length;

    public int TimeStamp { get; private set; }


    public SimulationController(string path, ISimDataAccess da, Type assigner, Type pathfinder, int length, double speed) : base(speed)
    {
        _logger = new Logger("default");

        Load(path, da, assigner);

        _logFileDataAccess = da.LDA;

        if (!pathfinder.IsSubclassOf(typeof(PathFinder)))
        {
            throw new ArgumentException("Type must be a subclass of PathFinder", nameof(pathfinder));
        }

        if (pathfinder.GetConstructor([]) is null)
        {
            throw new ArgumentException("Type must have a constructor parameter without parameters", nameof(pathfinder));
        }

        _pathFinder = (PathFinder)Activator.CreateInstance(pathfinder)!;

        if (length == -1)
        {
            Length = double.PositiveInfinity;
        }
        else
        {
            Length = length;
        }
    }

    private void OnEnded(object? sender, EventArgs e)
    {
        Timer.Change(Timeout.Infinite, Timeout.Infinite);

        SaveLog();
    }

    private async void Load(string path, ISimDataAccess da, Type assigner)
    {
        await Task.Run(async () =>
        {
            Config config = await da.CDA.LoadAsync(path);

            _logger = new Logger(config.MapFile.Split('/')[^1].Replace(".map", ""));

            Board b = await da.BDA.LoadAsync(config.MapFile);
            _board = b; // for some reason it only sets board this way ????????

            _robots.AddRange(await da.RDA.LoadAsync(config.AgentFile, _board.Width - 2, _board.Height - 2));
            _logger.LogStarts(_robots);

            List<Package> tasks = await da.PDA.LoadAsync(config.TaskFile, _board.Width - 2, _board.Height - 2);
            _logger.LogTasks(tasks);

            if (!assigner.IsSubclassOf(typeof(Assigner.Assigner)))
            {
                throw new ArgumentException("Type must be a subclass of Assigner", nameof(assigner));
            }

            if (assigner.GetConstructor([typeof(Board), typeof(IEnumerable<Package>), typeof(IEnumerable<Robot>)]) is null)
            {
                throw new ArgumentException($"Type must have a constructor with a {typeof(Board)}, a {typeof(IEnumerable<Package>)} and a {typeof(IEnumerable<Robot>)} parameter", nameof(assigner));
            }

            _assigner = (Assigner.Assigner)Activator.CreateInstance(assigner, b, tasks, _robots)!;
            _assigner.Ended += OnEnded;

            foreach (Robot r in _robots) Assign(r);

            LoadWalls();

            OnLoaded(this);
            
        }).ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                OnException(this, t.Exception);
            }
        });
    }

    public async void SaveLog()
    {
        _logger.LogActualPaths(_robots);

        _logger.LogReplayLength(_assigner!.TimeStamp + 1);

        await _logger.SaveAsync(_logFileDataAccess);
    }

    protected override void OnTick(object? state)
    {
        Step();

        CallTick(this);
    }

    public override void StepForward()
    {
        if (!IsPlaying) OnTick(null);
    }

    private void Step()
    {
        lock (_board)
        {
            if (_assigner!.NoPackage && _paths.All(x => x.Value.IsOver) || TimeStamp >= Length)
            {
                foreach (Robot? robot in _robots.Where(r => r.Task is not null))
                {
                    robot.RemoveTask();
                }

                OnEnded(this, EventArgs.Empty);
                return;
            }

            foreach (Robot robot in _robots)
            {
                if (!_paths.TryGetValue(robot, out Path? path))
                {
                    // sometimes this is caused by pathfinding taking too long and it times out
                    throw new System.Exception("");
                }

                if (path is null)
                {
                    throw new System.Exception("");
                }

                if (path.IsOver)
                {
                    if (robot.Task != null)
                    {
                        _logger.LogPlannerPaths(robot.ID, path);
                        _logger.LogFinish(robot.ID, robot.Task!.ID, TimeStamp);
                    }

                    if (!_paths.TryRemove(robot, out _))
                    {
                        throw new System.Exception("");
                    }

                    path = Assign(robot);
                }

                if (!path.IsOver)
                {
                    Action action = path.PeekNext();

                    if (robot.TryStep(action, _board, TimeStamp))
                    {
                        path.Increment();
                    }
                    else
                    {
                        Free(robot, path);
                        _board.Reserve(robot.Position, TimeStamp + 1);
                    }
                }
                else
                {
                    // this could happen if the next task is at the same position as the robot which is assigned to
                    _board.UnReserve(robot.Position, TimeStamp);
                }
            }

            TimeStamp++;
        }
    }

    private Path Assign(Robot robot)
    {
        Package? task = null;
        Point? task_pos = null;
        bool found;
        List<Action> actions = [Action.W];

        if (_assigner!.Peek(robot, out Package? package))
        {
            (found, actions) = _pathFinder.CalculatePath(_board, robot.Position, (int)robot.Direction, package.Position, TimeStamp);

            if (found)
            {
                task = package;
                task_pos = package.Position;
                _assigner.Get(robot, out _);
                _logger.LogAssignment(robot.ID, package.ID, TimeStamp);
            }
            else
            {
                _board.Reserve(robot.Position, TimeStamp + 1);
            }

        }
        else
        {
            // no package
            _board.Reserve(robot.Position, TimeStamp + 1);
        }

        robot.AddTask(task);

        Path path = new(actions, task_pos);

        if (!_paths.TryAdd(robot, path))
        {
            throw new System.Exception("");
        }

        return path;
    }

    private void Free(Robot robot, Path path)
    {
        if (!path.FreeAllReserved(_board, robot.Position, robot.Direction, TimeStamp))
        {
            throw new System.Exception("");
        }

        if (!_paths.TryRemove(robot, out _))
        {
            throw new System.Exception("");
        }

        if (!_paths.TryAdd(robot, new Path([], null)))
        {
            throw new System.Exception("");
        }

        Package package = robot.RemoveTask();

        _assigner!.Return(package);
    }

    public void Assign(Robot robot, Point target)
    {
        if (_paths.TryGetValue(robot, out Path? path) && !path.IsOver) Free(robot, path);

        _paths.TryRemove(robot, out _);

        Package? task = null;
        bool found;
        List<Action> actions;

        (found, actions) = _pathFinder.CalculatePath(_board, robot.Position, (int)robot.Direction, target, TimeStamp);

        if (found)
        {
            task = new Package(target);

            _logger.LogAssignment(robot.ID, task.ID, TimeStamp);
        }
        else
        {
            _board.Reserve(robot.Position, TimeStamp + 1);
        }

        robot.AddTask(task);

        path = new(actions, target);

        if (!_paths.TryAdd(robot, path))
        {
            throw new System.Exception("");
        }

        CallTick(this);
    }
}
