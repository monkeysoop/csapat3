namespace Mekkdonalds.Simulation.PathFinding;

public abstract class PathFinder
{
    protected static readonly Point[] nexts_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    internal (bool, List<Action>) CalculatePath(Board board, Point start_position, int start_direction, Point end_position, int start_cost)
    {
        bool found;
        int[] parents_data;
        int[] costs_data;

        (found, parents_data, costs_data) = FindPath(board, start_position, start_direction, end_position, start_cost);
        board.ClearMask();

        if (found)
        {
            return (true, TracePath(parents_data, costs_data, board, start_position, end_position));
        }
        else
        {
            return (false, new List<Action>());
        }
    }    

    private static List<Action> TracePath(int[] parents_board, int[] costs_board, Board board, Point start, Point end)
    {
        List<Action> path = [];


        Point current_position = end;
        int current_direction = (parents_board[current_position.Y * board.Width + current_position.X] + 2) % 4;

        while (!ComparePoints(current_position, start))
        {
            Point next_offset = nexts_offsets[current_direction];

            Point next_position = new(current_position.X + next_offset.X,
                                      current_position.Y + next_offset.Y);

            int next_direction = (parents_board[next_position.Y * board.Width + next_position.X] + 2) % 4;

            int diff = current_direction - next_direction;


            path.Add(Action.F);
            switch (diff)
            {
                case -3:
                    path.Add(Action.R);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2); // for stopping robots clipping trough each other
                    break;
                case -2:
                    path.Add(Action.R);
                    path.Add(Action.R);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 3); // for stopping robots clipping trough each other
                    break;
                case -1:
                    path.Add(Action.C);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2); // for stopping robots clipping trough each other
                    break;
                case 0:
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1); // for stopping robots clipping trough each other
                    break;
                case 1:
                    path.Add(Action.R);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2); // for stopping robots clipping trough each other
                    break;
                case 2:
                    path.Add(Action.R);
                    path.Add(Action.R);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 3); // for stopping robots clipping trough each other
                    break;
                case 3:
                    path.Add(Action.C);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2); // for stopping robots clipping trough each other
                    break;
            }

            current_position = next_position;
            current_direction = next_direction;
        }
        board.Reserve(start, costs_board[start.Y * board.Width + start.X]);
        //board.Reserve(end, costs_board[end.Y * board.Width + end.X] + 1);

        path.Reverse();

        return path;
    }

    protected abstract (bool, int[], int[]) FindPath(Board board, Point start_position, int start_direction, Point end_position, int start_cost);

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

        if (diff_x == 0 && diff_y == 0)
        {
            return 0;
        } else if (dot_product * dot_product == (diff_x * diff_x + diff_y * diff_y) * 1) // note that direction is a unitvector so its length is 1
        {
            return 0;
        } else if (dot_product * dot_product == -1 * (diff_x * diff_x + diff_y * diff_y) * 1) // note that direction is a unitvector so its length is 1
        {
            return 2;
        } else if (dot_product > 0)
        {
            return 1;
        } else if (dot_product < 0)
        {
            return 2;
        } else if (dot_product == 0)
        {
            return 1;
        } else
        {
            throw new ArgumentException();
        }

        //if (dot_product > 0 || (diff_x == 0 && diff_y == 0))
        //{
        //    return 0;
        //} else if (dot_product * dot_product != -1 * (diff_x * diff_x + diff_y * diff_y) * 1) // note that direction is a unitvector so its length is 1
        //{
        //    return 1;
        //} else
        //{
        //    //Debug.WriteLine("position:" + position);
        //    //Debug.WriteLine("end:" + end);
        //    //Debug.WriteLine("direction:" + direction);
        //    //Debug.WriteLine("diff:" + new Point(diff_x, diff_y));
        //    //Debug.WriteLine("dot product:" + dot_product);
        //    return 2;
        //}
    }
}
