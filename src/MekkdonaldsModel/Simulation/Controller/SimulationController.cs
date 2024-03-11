using System.Drawing;
using System.Numerics;

namespace Mekkdonalds.Simulation.Controller;

internal abstract class SimulationController : Controller
{
    protected SimulationController(List<Robot> r, double interval) : base(r, interval)
    {
        var tasks = new List<Task>();
        Robots.ForEach(x => tasks.Add(CalculatePath(x)));

        Task.WaitAll([.. tasks]);
    }

    protected abstract Task CalculatePath(Robot robot);
    
    protected override void OnTick(object? state)
    {
        throw new NotImplementedException();
    }

    protected int ManhattenDistance(Point start, Point end)
    {
        return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
    }

    protected int MaxTurnsRequired(Point position, Point direction, Point end)
    {
        int diff_x = end.X - position.X;
        int diff_y = end.Y - position.Y;
        int dot_product = diff_x * direction.X + diff_y * direction.Y;

        if (dot_product < 0)
        {
            return 2;
        } else if (dot_product * dot_product != diff_x * diff_x + diff_y * diff_y)
        {
            return 1;
        } else { 
            return 0; 
        }
    }
}
