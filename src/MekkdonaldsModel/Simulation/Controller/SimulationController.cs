using System.Diagnostics;

namespace Mekkdonalds.Simulation.Controller;

public abstract class SimulationController
{
    private static readonly string[] turns = ["FR", "FRR", "FL", "F", "FR", "FRR", "FL"]; // RR could be replaced with LL (this is just turning 180)
    
    protected static readonly Point[] nexts_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    protected abstract (bool, int[]) FindPath(Board2 board, Point start_position, int start_direction, Point end_position);

    public (bool, string) CalculatePath(Board2 board, Point start_position, int start_direction, Point end_position)
    {
        bool found;
        int[] parents_data;
        (found, parents_data) = FindPath(board, start_position, start_direction, end_position);
        if (found)
        {
            return (true, TracePath(parents_data, board.Width, start_position, start_direction, end_position));
        } else
        {
            return (false, "");
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

    private static string TracePath(int[] parents_board, int board_width, Point start, int start_direction, Point end)
    {
        string path = "";


        Point current_position = end;
        int current_direction = (parents_board[current_position.Y * board_width + current_position.X] + 2) % 4;

        while (!ComparePoints(current_position, start))
        {
            Point next_offset = nexts_offsets[current_direction];

            Point next_position = new(current_position.X + next_offset.X,
                                      current_position.Y + next_offset.Y);

            int next_direction = (parents_board[next_position.Y * board_width + next_position.X] + 2) % 4;

            int diff = current_direction - next_direction + 3;
            path += turns[diff];

            current_position = next_position;
            current_direction = next_direction;
        }

        System.Diagnostics.Debug.WriteLine("path in reverse: " + path);
        return path;
    }

}
