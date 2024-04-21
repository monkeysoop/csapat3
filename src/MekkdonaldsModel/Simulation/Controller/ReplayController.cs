﻿namespace Mekkdonalds.Simulation.Controller;

public sealed class ReplayController : Controller
{
    private readonly ConcurrentDictionary<Robot, List<Action>> Paths = [];
    private readonly ConcurrentDictionary<Robot, IntervalTree<Point?>> Targets = [];

    public int TimeStamp { get; private set; }
    public int Length { get; private set; }

    public ReplayController(string logPath, string mapPath, IReplayDataAccess da)
    {
        Load(logPath, mapPath, da);
    }

    private void Load(string logPath, string mapPath, IReplayDataAccess da)
    {
        Task.Run(async () =>
        {
            _board = await da.BDA.LoadAsync(mapPath);

            var log = await da.LDA.LoadAsync(logPath);

            foreach (var (p, d) in log.Start)
            {
                var r = new Robot(p, d);
                Paths[r] = [];
                Targets[r] = [];
                _robots.Add(r);
            }

            for (int i = 0; i < log.ActualPaths.Count; i++)
            {
                Paths[_robots[i]].AddRange(log.ActualPaths[i]);
            }

            var width = Width - 2;

            int start;

            for (int i = 0; i < log.Events.Count; i++)
            {
                var r = _robots[i];

                start = 0;

                foreach (var (task, t, e) in log.Events[i])
                {
                    switch (e)
                    {
                        case "assigned":
                            start = t;
                            break;
                        case "finished":
                            var pos = log.Tasks.First(x => x.Item1 == task);
                            var p = new Point(pos.Item3 + 1, pos.Item2 + 1);
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

    protected override void OnTick(object? state)
    {
        if (TimeStamp == Length)
        {
            Timer.Change(Timeout.Infinite, Timeout.Infinite);
            IsPlaying = false;
            return;
        }

        StepForward();
    }

    public void JumpTo(int time)
    {
        if (time < 0 || time > Length) throw new ArgumentOutOfRangeException(nameof(time), "Time must be between 0 and the length of the replay");

        foreach (var r in _robots)
        {
            r.AddTask(Targets[r][time]);

            if (Math.Sign(time - TimeStamp) == -1)
            {
                for (int t = TimeStamp - 1; t >= time; t--)
                {
                    var a = Paths[r][t];
                    r.Step(a.Reverse());
                }
            }
            else if (Math.Sign(time - TimeStamp) == 1)
            {
                for (int t = TimeStamp; t < time; t++)
                {
                    var a = Paths[r][t];
                    r.Step(a);
                }
            }
        }

        TimeStamp = time;

        CallTick(this);
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
