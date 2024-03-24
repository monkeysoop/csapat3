namespace Mekkdonalds.Simulation.Controller;

public sealed class SimulationController : Controller
{
    protected static readonly Point[] nexts_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    protected abstract (bool, int[]) FindPath(Board2 board, Point start_position, int start_direction, Point end_position);

    public (bool, List<Action>) CalculatePath(Board2 board, Point start_position, int start_direction, Point end_position)
    {
        bool found;
        int[] parents_data;

        (found, parents_data) = FindPath(board, start_position, start_direction, end_position);
        board.ClearMask();

        if (found)
        {
            return (true, TracePath(parents_data, board.Width, start_position, start_direction, end_position));
        } else
        {
            return (false, new List<Action>());
        }
    }

    public void FindAllPaths(Board2 board, List<Robot> robots, List<Package> packages)
    {
        while (packages.Count > 0)
        {
            foreach(Robot r  in robots)
            {
                
            }
        }
    }

    public static List<Action> Convert(string path)
    {
        var l = new List<Action>();

        foreach (char c in path)
        {
            switch (c)
            {
                case 'F':
                    l.Add(Action.F);
                    break;
                case 'R':
                    l.Add(Action.R);
                    break;
                case 'L':
                    l.Add(Action.C);
                    break;
                case 'W':
                    l.Add(Action.W);
                    break;
                default: // can be removed
                    throw new PathException($"Invalid charachter {c}");
            }
        }

    protected override void OnTick(object? state)
            {
        Debug.WriteLine("Tick");
    }

}
