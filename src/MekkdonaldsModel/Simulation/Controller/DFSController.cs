namespace Mekkdonalds.Simulation.Controller;

public sealed class DFSController(double interval) : SimulationController(interval)
{
    public DFSController() : this(1) { }

    static private bool DFSPathFinder(Board2 board, Point start, int start_direction, Point end)
    {
        // this depth first search uses heuristics to hopefully find a correct path quicker
        Step[] stack = new Step[5 * board.Height * board.Width];


        stack[0] = new Step(start, start_direction, 0);
        int stack_index = 1;


        int[] parents = new int[board.Height * board.Width]; // all items are automatically set to 0


        bool found = false;
        while(stack_index != 0 && !found)
        {
            stack_index--;
            Step current_step = stack[stack_index];
            if (board.SetSearchedIfEmpty(current_step.position))
            {
                parents[current_step.position.Y * board.Width + current_step.position.X] = current_step.direction;
                if (ComparePoints(current_step.position, end))
                {
                    found = true;
                } else
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

                    int heuristic1 = MaxTurnsRequired(forward_next_position, forward_offset, end) + ManhattenDistance(forward_next_position, end);
                    int heuristic2 = MaxTurnsRequired(left_next_position, left_offset, end) + ManhattenDistance(left_next_position, end) + 1;
                    int heuristic3 = MaxTurnsRequired(right_next_position, right_offset, end) + ManhattenDistance(right_next_position, end) + 1;

                    int index_offset1 = 2;
                    int index_offset2 = 1;
                    int index_offset3 = 0;

                    int t;
                    if (heuristic1 > heuristic2)
                    {
                        t = heuristic1;
                        heuristic1 = heuristic2;
                        heuristic2 = t;
                        index_offset1 = 1;
                        index_offset2 = 2;
                    }
                    if (heuristic1 > heuristic3)
                    {
                        t = heuristic1;
                        heuristic1 = heuristic3;
                        heuristic3 = t;
                        index_offset3 = index_offset1;
                        index_offset1 = 0;
                    }
                    if (heuristic2 > heuristic3)
                    {
                        t = heuristic2;
                        heuristic2 = heuristic3;
                        heuristic3 = t;
                        t = index_offset2;
                        index_offset2 = index_offset3;
                        index_offset3 = t;
                    }

                    stack[stack_index + index_offset1] = new Step (forward_next_position, forward_direction, heuristic1);
                    stack[stack_index + index_offset2] = new Step (left_next_position, left_direction, heuristic2);
                    stack[stack_index + index_offset3] = new Step (right_next_position, right_direction, heuristic3);

                    stack_index += 3;
                }
            }
        }
        return found;
    }
    protected override Task CalculatePath(Robot robot)
    {
        Paths[robot] = new Path();

        return Task.CompletedTask;
    }
}
