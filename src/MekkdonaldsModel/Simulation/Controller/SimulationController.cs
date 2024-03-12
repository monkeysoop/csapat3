namespace Mekkdonalds.Simulation.Controller;

public abstract class SimulationController : Controller
{
    protected static readonly Point[] nexts_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];
    private static readonly string[] turns = {"FR", "FRR", "FL", "F", "FR", "FRR", "FL"}; // RR could be replaced with LL (this is just turning 180)

    public int Cost { get; protected set; } // Apperently 32bit value types are atomic in c# by default
    private TimeSpan Elapsed;

    protected SimulationController(double interval) : base()
    {
        var tasks = new List<Task>();

        _robots.AddRange([new(1, 0, 0), new(2, 10, 25), new(3, 2, 2)]);
        _walls.AddRange([new(1, 1), new(1, 3)]);

        _robots.ForEach(x => tasks.Add(CalculatePath(x)));

        Task.WaitAll([.. tasks]);

        Timer.Change(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(interval));
    }

    protected abstract Task CalculatePath(Robot robot);

    protected override void OnTick(object? state)
    {
        Task.Run(() => { _robots.ForEach(r => r.Step(Paths[r].Next())); });
        Elapsed += new TimeSpan(0, 0, 1);

        CallTick(this);
    }


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

    protected static void TracePath(int[] parents_board, int board_width, Point start, int start_direction, Point end)
    {
        string path = "";
        
        Point current_position = end;
        int current_direction = (parents_board[end.Y * board_width + end.X] + 2) % 4;
        while (ComparePoints(current_position, start))
        {
            int next_direction = parents_board[current_position.Y * board_width + current_position.X];
            int diff = next_direction - current_direction + 3;

            path += turns[diff];

            Point next_offset = nexts_offsets[next_direction];
            current_position = new(current_position.X + next_offset.X,
                                   current_position.Y + next_offset.Y);

            current_direction = next_direction;
        }

    }
}
