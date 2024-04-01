namespace Mekkdonalds.Simulation.PathFinding;

public sealed class BFS : PathFinder
{
    protected override (bool, int[]) FindPath(Board2 board, Point start_position, int start_direction, Point end_position)
    {
        Step[] queue = new Step[5 * board.Height * board.Width];
        int start_index = 0;
        int end_index = 0;
        int[] parents = new int[board.Height * board.Width]; // all items are automatically set to 0



        queue[0] = new Step(start_position, start_direction, 0);
        end_index++;

        parents[start_position.Y * board.Width + start_position.X] = start_direction;


        int backward_direction = (start_direction + 2) % 4;
        Point backward_offset = nexts_offsets[backward_direction];
        Point backward_next_position = new(start_position.X + backward_offset.X,
                                           start_position.Y + backward_offset.Y);

        if (board.SetSearchedIfEmpty(backward_next_position))
        {
            queue[1] = new Step(backward_next_position, backward_direction, 0);
            end_index++;
            parents[backward_next_position.Y * board.Width + backward_next_position.X] = backward_direction;
        }


        bool found = false;
        while (start_index != end_index && !found)
        {
            Step current_step = queue[start_index];
            start_index++;

            if (ComparePoints(current_step.Position, end_position))
            {
                found = true;
            }
            else
            {
                int forward_direction = current_step.Direction;
                int left_direction = (current_step.Direction + 3) % 4;
                int right_direction = (current_step.Direction + 1) % 4;

                Point forward_offset = nexts_offsets[forward_direction];
                Point left_offset = nexts_offsets[left_direction];
                Point right_offset = nexts_offsets[right_direction];

                Point forward_next_position = new(current_step.Position.X + forward_offset.X,
                                                  current_step.Position.Y + forward_offset.Y);
                Point left_next_position = new(current_step.Position.X + left_offset.X,
                                               current_step.Position.Y + left_offset.Y);
                Point right_next_position = new(current_step.Position.X + right_offset.X,
                                                current_step.Position.Y + right_offset.Y);

                if (board.SetSearchedIfEmpty(forward_next_position))
                {
                    queue[end_index] = new Step(forward_next_position, forward_direction, 0);
                    end_index++;
                    parents[forward_next_position.Y * board.Width + forward_next_position.X] = forward_direction;
                }
                if (board.SetSearchedIfEmpty(left_next_position))
                {
                    queue[end_index] = new Step(left_next_position, left_direction, 0);
                    end_index++;
                    parents[left_next_position.Y * board.Width + left_next_position.X] = left_direction;
                }
                if (board.SetSearchedIfEmpty(right_next_position))
                {
                    queue[end_index] = new Step(right_next_position, right_direction, 0);
                    end_index++;
                    parents[right_next_position.Y * board.Width + right_next_position.X] = right_direction;
                }
            }
        }
        return (found, parents);
    }
}
