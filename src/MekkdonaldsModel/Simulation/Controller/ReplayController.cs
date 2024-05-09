namespace Mekkdonalds.Simulation.Controller;

public sealed class ReplayController : Controller
{
    private readonly ConcurrentDictionary<Robot, List<Action>> Paths = [];
    private readonly ConcurrentDictionary<Robot, IntervalTree<Point?>> Targets = [];

    /// <summary>
    /// Length of the replay
    /// </summary>
    public int Length { get; private set; }

    /// <summary>
    /// constructor for the replay controller
    /// </summary>
    /// <param name="logPath">path to the log file</param> 
    /// <param name="mapPath"> path to the map file</param>
    /// <param name="da">data access object</param> 
    public ReplayController(string logPath, string mapPath, IReplayDataAccess da)
    {
        Load(logPath, mapPath, da);
    }

    /// <summary>
    /// loads the log and map files
    /// </summary>
    /// <param name="logPath"> path to the log file</param>
    /// <param name="mapPath">path to the map file</param> 
    /// <param name="da">data access object</param> 
    private async void Load(string logPath, string mapPath, IReplayDataAccess da)
    {
        await Task.Run(async () =>
        {
            _board = await da.BDA.LoadAsync(mapPath);

            LogFile log = await da.LDA.LoadAsync(logPath);

            foreach (var (p, d) in log.Start)
            {
                Robot r = new(p, d);
                Paths[r] = [];
                Targets[r] = [];
                _robots.Add(r);
            }

            for (int i = 0; i < log.ActualPaths.Count; i++)
            {
                Paths[_robots[i]].AddRange(log.ActualPaths[i]);
            }

            int width = Width - 2;

            int start;

            for (int i = 0; i < log.Events.Count; i++)
            {
                Robot r = _robots[i];

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
                            Point p = new(pos.Item3 + 1, pos.Item2 + 1);
                            Targets[r][start, t] = p;
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

    /// <summary>
    /// implementation of the OnTick method
    /// method to be called on each tick
    /// </summary>
    /// <param name="state">state</param>
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
    /// jumps to a specific time
    /// </summary>
    /// <param name="time">time to jump to</param> 
    /// <exception cref="ArgumentOutOfRangeException"> throws an exception if the time is out of bounds</exception>
    public void JumpTo(int time)
    {
        if (time < 0 || time > Length) throw new ArgumentOutOfRangeException(nameof(time), "Time must be between 0 and the length of the replay");

        lock (this)
        {
            foreach (var r in _robots)
            {
                r.AddTask(Targets[r][time]);

                if (Math.Sign(time - TimeStamp) == -1)
                {
                    for (int t = TimeStamp - 1; t >= time; t--)
                    {
                        try
                        {
                            Action a = Paths[r][t];
                            r.Step(a.Reverse());
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
                            Action a = Paths[r][t];
                            r.Step(a);
                        }
                        catch (System.Exception) { }
                    }
                }
            }

            TimeStamp = time;
        }

        CallTick(this);
    }

    /// <summary>
    /// implementation of the StepForward method
    /// </summary>
    public override void StepForward()
    {
        JumpTo(TimeStamp + 1);
    }
    /// <summary>
    /// method to step backward
    /// </summary>
    public void StepBackward()
    {
        JumpTo(TimeStamp - 1);
    }
}
