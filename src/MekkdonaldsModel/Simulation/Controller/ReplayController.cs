namespace Mekkdonalds.Simulation.Controller;

public sealed class ReplayController : Controller
{
    private readonly ConcurrentDictionary<Robot, List<Action>> Paths = [];
    private readonly ConcurrentDictionary<Robot, IntervalTree<Point?>> Targets = [];

    /// <summary>
    /// Which step the controller is currently at.
    /// </summary>
    public int TimeStamp { get; private set; }
    /// <summary>
    /// Length of the replay.
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// Initializes a new <see cref="Controller"/> that handles replaying a simulation.
    /// </summary>
    /// <param name="logPath">Path of the log file</param>
    /// <param name="mapPath">Path of the map file (will not be checked for size match, collisions, etc.)</param>
    /// <param name="dataAccess">Preferred data access classes</param>
    public ReplayController(string logPath, string mapPath, IReplayDataAccess dataAccess)
    {
        Load(logPath, mapPath, dataAccess);
    }

    private async void Load(string logPath, string mapPath, IReplayDataAccess dataAccess)
    {
        await Task.Run(async () =>
        {
            _board = await dataAccess.BoardDataAccess.LoadAsync(mapPath);

            LogFile log = await dataAccess.LogFileDataAccess.LoadAsync(logPath);

            foreach (var (p, d) in log.Start)
            {
                Robot robot = new(p, d);
                Paths[robot] = [];
                Targets[robot] = [];
                _robots.Add(robot);
            }

            for (int i = 0; i < log.ActualPaths.Count; i++)
            {
                Paths[_robots[i]].AddRange(log.ActualPaths[i]);
            }

            int width = Width - 2;

            int start;

            for (int i = 0; i < log.Events.Count; i++)
            {
                Robot robot = _robots[i];

                start = 0;

                foreach (var (task, t, e) in log.Events[i])
                {
                    switch (e)
                    {
                        case "assigned":
                            start = t;
                            break;
                        case "finished":
                            (int, int, int) pos = log.Tasks.First(x => x.Item1 == task);
                            Point point = new(pos.Item3 + 1, pos.Item2 + 1);
                            Targets[robot][start, t] = point;
                            start = -1;
                            break;
                        default:
                            break;
                    }
                }
            }

            Length = log.Makespan;

            LoadWalls();

            OnLoaded(this);
        });
    }

    protected override void OnTick(object? state)
    {
        if (TimeStamp >= Length)
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            IsPlaying = false;
            return;
        }

        StepForward();
    }

    /// <summary>
    /// Jumps to a specific time in the replay.
    /// </summary>
    /// <param name="time">Timestamp to jump to</param>
    /// <exception cref="ArgumentOutOfRangeException">Gets thrown when the <paramref name="time"/> is less then zero or greater then the length of the replay</exception>
    public void JumpTo(int time)
    {
        if (time < 0 || time > Length) throw new ArgumentOutOfRangeException(nameof(time), "Time must be between 0 and the length of the replay");

        lock (this)
        {
            foreach (var robot in _robots)
            {
                robot.AddTask(Targets[robot][time]);

                if (Math.Sign(time - TimeStamp) == -1)
                {
                    for (int t = TimeStamp - 1; t >= time; t--)
                    {
                        try
                        {
                            Action action = Paths[robot][t];
                            robot.Step(action.Reverse());
                        }
                        catch (System.Exception) { }
                    }
                }
                else if (Math.Sign(time - TimeStamp) == 1)
                {
                    for (int t = TimeStamp; t < time; t++)
                    {
                        try
                        {
                            Action action = Paths[robot][t];
                            robot.Step(action);
                        }
                        catch (System.Exception) { }
                    }
                }
            }

            TimeStamp = time;
        }

        CallTick(this);
    }

    public override void StepForward()
    {
        if (TimeStamp >= Length) return;

        JumpTo(TimeStamp + 1);
    }

    /// <summary>
    /// Steps backward by one step.
    /// </summary>
    public void StepBackward()
    {
        if (TimeStamp <= 0) return;

        JumpTo(TimeStamp - 1);
    }
}
