namespace Mekkdonalds.Simulation.Controller;

public sealed class SimulationController : Controller
{
    private Assigner.Assigner? _assigner;

    private readonly ConcurrentDictionary<Robot, Path> _paths = [];
    private readonly PathFinder _pathFinder = new AStar();

    private Logger _logger;
    private readonly ILogFileDataAccess _logFileDataAccess;

    private CancellationTokenSource? _cancellationTokenSource;

    private readonly double _length;

    public int TimeStamp { get; private set; }
    public bool IsOver { get; private set; }

    /// <summary>
    /// Occurs when the simulation has ended (all the task are completed or the desired length has been reached).
    /// </summary>
    public event EventHandler? Ended;

    /// <summary>
    /// Initializes a new <c>Controller</c> that handles a simulation.
    /// </summary>
    /// <param name="path">Path of the configuration file</param>
    /// <param name="dataAccess">Preferred data access classes</param>
    /// <param name="assigner">Class that will handle the assignment of packages (has to implement the <c>Assigner</c> abstract class)</param>
    /// <param name="pathFinder">Class that will handle the path planning of robots (has to implement the <c>PathFinder</c> abstract class)</param>
    /// <param name="speed">Intervals of simulation steps (in seconds)</param>
    /// <param name="length">Desired length of the simulation</param>
    /// <exception cref="ArgumentException">Gets thrown when the <paramref name="pathFinder"/> is not a subclass of <c>PathFinder</c></exception>
    public SimulationController(string path, ISimDataAccess dataAccess, Type assigner, Type pathFinder, double speed, int length) : base(speed)
    {
        _logger = new Logger("default");

        Load(path, dataAccess, assigner);

        _logFileDataAccess = dataAccess.LogFileDataAccess;

        if (!pathFinder.IsSubclassOf(typeof(PathFinder)))
        {
            throw new ArgumentException("Type must be a subclass of PathFinder", nameof(pathFinder));
        }

        if (pathFinder.GetConstructor([]) is null)
        {
            throw new ArgumentException("Type must have a constructor parameter without parameters", nameof(pathFinder));
        }

        _pathFinder = (PathFinder)Activator.CreateInstance(pathFinder)!;

        if (length == -1)
        {
            _length = double.PositiveInfinity;
        }
        else
        {
            _length = length;
        }
    }

    /// <summary>
    /// Initializes a new <c>Controller</c> that handles a simulation.
    /// </summary>
    /// <param name="path">Path of the configuration file</param>
    /// <param name="dataAccess">Preferred data access classes</param>
    /// <param name="assigner">Class that will handle the assignment of packages (has to implement the <c>Assigner</c> abstract class)</param>
    /// <param name="pathFinder">Class that will handle the path planning of robots (has to implement the <c>PathFinder</c> abstract class)</param>
    /// <param name="speed">Intervals of simulation steps (in seconds)</param>
    /// <exception cref="ArgumentException">Gets thrown when the pathFinder is not a subclass of <c>PathFinder</c></exception>
    public SimulationController(string path, ISimDataAccess dataAccess, Type assigner, Type pathFinder, double speed) : this(path, dataAccess, assigner, pathFinder, speed, -1) { }

    private void OnEnded()
    {
        Timer.Change(Timeout.Infinite, Timeout.Infinite);

        SaveLog();

        Ended?.Invoke(this, EventArgs.Empty);
    }

    private async void Load(string path, ISimDataAccess da, Type assigner)
    {
        await Task.Run(async () =>
        {
            Config config = await da.ConfigDataAccess.LoadAsync(path);

            _logger = new Logger(config.MapFile.Split('/')[^1].Replace(".map", ""));

            Board b = await da.BoardDataAccess.LoadAsync(config.MapFile);
            _board = b; // for some reason it only sets board this way ????????

            _robots.AddRange(await da.RobotsDataAccess.LoadAsync(config.AgentFile, _board.Width - 2, _board.Height - 2));
            _logger.LogStarts(_robots);

            List<Package> tasks = await da.PackagesDataAccess.LoadAsync(config.TaskFile, _board.Width - 2, _board.Height - 2);
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

    /// <summary>
    /// Saves the log of the simulation in the current state.
    /// </summary>
    public async void SaveLog()
    {
        _logger.LogActualPaths(_robots);

        _logger.LogReplayLength(TimeStamp);

        await _logger.SaveAsync(_logFileDataAccess);
    }

    protected override void OnTick(object? state)
    {
        if (IsOver) return;

        _cancellationTokenSource = new CancellationTokenSource();

        var task = Task.Run(() =>
        {
            List<Robot>.Enumerator enumerator = _robots.GetEnumerator();

            Robot robot;

            lock (_board)
            {
                try
                {
                    if (_assigner!.NoPackage && _paths.All(x => x.Value.IsOver) || TimeStamp >= _length)
                    {
                        IsOver = true;
                        foreach (var r in _robots.Where(r => r.Task is not null))
                        {
                            Package task = r.RemoveTask();
                            _logger.LogFinish(r.ID, task.ID, TimeStamp);
                        }

                        OnEnded();
                        return;
                    }

                    while (enumerator.MoveNext())
                    {
                        robot = enumerator.Current;

                        if (!_paths.TryGetValue(robot, out Path? path))
                        {
                            // sometimes this is caused by pathfinding taking too long and it times out
                            throw new System.Exception("");
                        }

                        if (path.IsOver)
                        {
                            _logger.LogPlannerPaths(robot.ID, path);

                            if (robot.Task is not null)
                            {
                                _logger.LogFinish(robot.ID, robot.Task.ID, TimeStamp);
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
                                robot.TryStep(Action.W, _board, TimeStamp);
                            }
                        }
                        else
                        {
                            // this could happen if the next task is at the same position as the robot which is assigned to
                            _board.UnReserve(robot.Position, TimeStamp);
                            robot.TryStep(Action.W, _board, TimeStamp);
                        }

                        _cancellationTokenSource.Token.ThrowIfCancellationRequested();
                    }
                }
                catch (OperationCanceledException)
                {
                    Debug.WriteLine("Timeout");
                    while (enumerator.MoveNext())
                    {
                        robot = enumerator.Current;

                        robot.TryStep(Action.T, _board, TimeStamp);

                        if (!_paths.TryGetValue(robot, out Path? path))
                        {
                            throw new System.Exception("Robot's path has been deleted");
                        }

                        path.Alter(Action.T);
                        path.Increment();
                    }
                }
                finally
                {
                    TimeStamp++;
                    enumerator.Dispose();
                    CallTick(this);
                }
            }
        }, _cancellationTokenSource.Token).ContinueWith(t =>
        {
            if (t.IsFaulted)
            {
                OnException(this, t.Exception);
            }
        });

        _cancellationTokenSource.CancelAfter(Interval.Milliseconds);
    }
    
    public override void StepForward()
    {
        if (!IsPlaying) OnTick(null);
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
        _logger.LogFinish(robot.ID, robot.Task!.ID, TimeStamp);

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

    /// <summary>
    /// Assigns a task to the robot if the provided target is reachable.
    /// </summary>
    /// <param name="robot">Robot that get's the task assigned</param>
    /// <param name="target">Location of the target [1..Width]x[1..Height]</param>
    /// <exception cref="System.Exception"></exception>
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
