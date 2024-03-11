namespace Mekkdonalds.Simulation.Controller;

public abstract class SimulationController : Controller
{
    public int Cost { get; protected set; } // Apperently 32bit value types are atomic in c# by default
    private TimeSpan Elapsed;

    protected SimulationController(double interval) : base()
    {
        var tasks = new List<Task>();

        _robots.AddRange([new(1, 0, 0), new(2, 10, 25), new(3, 2, 2)]);
        _walls.AddRange([new(1, 1), new(1, 3)]);

        _robots.ForEach(x => tasks.Add(CalculatePath(x)));

        Task.WaitAll([.. tasks]);

        Timer.Change(TimeSpan.FromSeconds(1), new TimeSpan(0, 0, 1));
    }

    protected abstract Task CalculatePath(Robot robot);

    protected override void OnTick(object? state)
    {
        Task.Run(() => { _robots.ForEach(r => r.Step(Paths[r].Next())); });
        Elapsed += new TimeSpan(0, 0, 1);

        CallTick(this);
    }

    protected static readonly Point[] nexts_offsets = {
        new Point(0, -1),
        new Point(1, 0),
        new Point(0, 1),
        new Point(-1, 0)
    };

    protected static bool ComparePoints(Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }

    protected static readonly Point[] nexts_offsets = {
        new Point(0, -1),
        new Point(1, 0),
        new Point(0, 1),
        new Point(-1, 0)
    };

    protected static bool ComparePoints(Point first, Point second)
    {
        return first.X == second.X && first.Y == second.Y;
    }

    protected static int ManhattenDistance(Point start, Point end)
    {
        return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
    }

    protected static int MaxTurnsRequired(Point position, Point direction, Point end)
    {
        int diff_x = end.X - position.X;
        int diff_y = end.Y - position.Y;
        int dot_product = diff_x * direction.X + diff_y * direction.Y;

        if (dot_product < 0)
        {
            return 2;
        }
        else if (dot_product * dot_product != diff_x * diff_x + diff_y * diff_y)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
