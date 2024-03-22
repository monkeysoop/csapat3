namespace Mekkdonalds.Simulation.Controller;

internal sealed class BFSController(double interval) : SimulationController(interval)
{
    public BFSController() : this(1) { }

    private bool BFSPathFinder(Point start, int start_direction, Point end)
    {
        Step[] queue = new Step[5 * _board.Height * _board.Width];


        queue[0] = new Step(start, start_direction, 0);
        int start_index = 0;
        int end_index = 1;


        int[] parents = new int[_board.Height * _board.Width]; // all items are automatically set to 0


        bool found = false;
        while (start_index != end_index && !found)
        {
            Step current_step = queue[start_index];
            start_index++;
            
            if (ComparePoints(current_step.position, end))
            {
                found = true;
            }
            else
            {
                int forward_direction = current_step.direction;
                int left_direction = (current_step.direction + 3) % 4;
                int right_direction = (current_step.direction + 1) % 4;

                Point forward_offset = nexts_offsets[forward_direction];
                Point left_offset = nexts_offsets[left_direction];
                Point right_offset = nexts_offsets[right_direction];

                Point forward_next_position = new(current_step.position.X + forward_offset.X,
                                                  current_step.position.Y + forward_offset.Y);
                Point left_next_position = new(current_step.position.X + left_offset.X,
                                               current_step.position.Y + left_offset.Y);
                Point right_next_position = new(current_step.position.X + right_offset.X,
                                                current_step.position.Y + right_offset.Y);

                if (_board.SetSearchedIfEmpty(forward_next_position))
                {
                    queue[end_index] = new Step(forward_next_position, forward_direction, 0);
                    end_index++;
                    parents[forward_next_position.Y * _board.Width + forward_next_position.X] = forward_direction;
                }
                if (_board.SetSearchedIfEmpty(left_next_position))
                {
                    queue[end_index] = new Step(left_next_position, left_direction, 0);
                    end_index++;
                    parents[left_next_position.Y * _board.Width + left_next_position.X] = left_direction;
                }
                if (_board.SetSearchedIfEmpty(right_next_position))
                {
                    queue[end_index] = new Step(right_next_position, right_direction, 0);
                    end_index++;
                    parents[right_next_position.Y * _board.Width + right_next_position.X] = right_direction;
                }
            }
        }
        return found;
    }
    
    protected override Task CalculatePath(Robot robot)
    {
        throw new NotImplementedException();
    }
}
