namespace Mekkdonalds.Simulation.Controller;

public abstract class PathFinder
{
    protected static readonly Point[] nexts_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    protected abstract (bool, int[]) FindPath(Board2 board, Point start_position, int start_direction, Point end_position);

    private  (bool, List<Action>) CalculatePath(Board2 board, Point start_position, int start_direction, Point end_position)
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
        int package_index = 0;
        while (package_index < packages.Count)
        {
            foreach(Robot r in robots)
            {
                if (!r.Available())
                {
                    r.Step();
                } else if (package_index < packages.Count)
                {
                    bool found;
                    List<Action> path;
                    (found, path) = CalculatePath(board, r.Position, (int)r.Direction, packages[package_index].Position);
                    
                    if (found)
                    {
                        r.AddPlannedRoute(path);
                        package_index++;
                        
                        r.Step();
                    } else
                    {
                        throw new PathException("no path found!");
                    }
                }
            }
        }
        foreach (Robot r in robots)
        {
            while (!r.Available())
            {
                r.Step();
            }
        }
        foreach (Robot r in robots)
        {
            Debug.WriteLine(string.Join(",", r.History.ToArray()));
        }
    }
    
    protected static bool ComparePoints(Point first, Point second) // == is overloaded
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

    private static List<Action> TracePath(int[] parents_board, int board_width, Point start, int start_direction, Point end)
    {
        List<Action> path = [];


        Point current_position = end;
        int current_direction = (parents_board[current_position.Y * board_width + current_position.X] + 2) % 4;

        while (!ComparePoints(current_position, start))
        {
            Point next_offset = nexts_offsets[current_direction];

            Point next_position = new(current_position.X + next_offset.X,
                                      current_position.Y + next_offset.Y);

            int next_direction = (parents_board[next_position.Y * board_width + next_position.X] + 2) % 4;

            int diff = current_direction - next_direction;


            path.Add(Action.F);
            switch (diff)
            {
                case -3: path.Add(Action.R); break;
                case -2: path.Add(Action.R); path.Add(Action.R); break;
                case -1: path.Add(Action.C); break;
                case  0: break;
                case  1: path.Add(Action.R); break;
                case  2: path.Add(Action.R); path.Add(Action.R); break;
                case  3: path.Add(Action.C); break;
            }

            current_position = next_position;
            current_direction = next_direction;
        }

        path.Reverse();

        return path;
    }

}
