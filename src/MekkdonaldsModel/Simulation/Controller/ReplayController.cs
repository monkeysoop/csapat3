namespace Mekkdonalds.Simulation.Controller;

public sealed class ReplayController : Controller
{
    private readonly ConcurrentDictionary<Robot, List<Action>> Paths = [];
    private readonly ConcurrentDictionary<Robot, IntervalTree<Point>> Targets = [];

    public int TimeStamp { get; private set; }
    public int Length { get; private set; }

    public ReplayController(string logPath, string mapPath, IReplayDataAccess da)
    {
        Load(logPath, mapPath, da);

        throw new NotImplementedException();
    }

    private async void Load(string logPath, string mapPath, IReplayDataAccess da)
    {
        _board = await da.BDA.LoadAsync(mapPath);

        LogFile log = (da as LogFile)!;

        foreach (var (p, d) in log.Start)
        {
            var r = new Robot(p, d);
            Paths[r] = [];
            Targets[r] = [];
            _robots.Add(r);
        }

        for (int i = 0; i < log.ActualPaths.Count; i++)
        {
            var path = log.ActualPaths[i];
            var actions = path.Split(' ').Select(s => (Action)Enum.Parse(typeof(Action), s)).ToList();
            Paths[_robots[i]].AddRange(actions);
        }

        var width = Width - 2;

        int start;

        for (int i = 0; i < log.Events.Count; i++)
        {
            var r = _robots[i];

            start = 0;

            foreach (var (pos, t, e) in log.Events[i])
            {
                switch (e)
                {
                    case "assigned":
                        start = t;
                        break;
                    case "finished":
                        var p = new Point((pos % width) + 1, (pos * width) + 1);
                        Targets[r][start, t] = p;
                        break;
                    default:
                        break;
                }
            }
        }

        throw new NotImplementedException();
    }

    protected override void OnTick(object? state)
    {
        CallTick(this);
    }

    public void JumpTo(int time)
    {
        if (time < 0 || time > Length) throw new ArgumentOutOfRangeException(nameof(time), "Time must be between 0 and the length of the replay");

        throw new NotImplementedException();
    }

    public override void StepForward()
    {
        JumpTo(TimeStamp + 1);
    }

    public void StepBackward()
    {
        JumpTo(TimeStamp - 1);
    }
}
