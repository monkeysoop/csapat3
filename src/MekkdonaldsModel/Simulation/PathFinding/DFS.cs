namespace Mekkdonalds.Simulation.PathFinding;

public sealed class DFS : PathFinder
{
    protected override (bool, int[]) FindPath(Board board, Point start_position, int start_direction, Point end_position)
    {
        // this depth first search uses heuristics to hopefully find a correct path quicker
        Step[] stack = new Step[5 * board.Height * board.Width];
        int stack_index = 0;
        int[] parents = new int[board.Height * board.Width]; // all items are automatically set to 0



        int backward_direction = (start_direction + 2) % 4;
        Point backward_offset = nexts_offsets[backward_direction];
        Point backward_next_position = new(start_position.X + backward_offset.X,
                                           start_position.Y + backward_offset.Y);

        stack[0] = new Step(backward_next_position, backward_direction, 0);
        stack_index++;


        stack[1] = new Step(start_position, start_direction, 0);
        stack_index++;

        parents[start_position.Y * board.Width + start_position.X] = start_direction;

        bool found = false;
        while (stack_index != 0 && !found)
        {
            stack_index--;
            Step current_step = stack[stack_index];
            if (board.SetSearchedIfEmpty(current_step.Position))
            {
                parents[current_step.Position.Y * board.Width + current_step.Position.X] = current_step.Direction;
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

                    int heuristic1 = MaxTurnsRequired(forward_next_position, forward_offset, end_position) + ManhattenDistance(forward_next_position, end_position);
                    int heuristic2 = MaxTurnsRequired(left_next_position, left_offset, end_position) + ManhattenDistance(left_next_position, end_position) + 1;
                    int heuristic3 = MaxTurnsRequired(right_next_position, right_offset, end_position) + ManhattenDistance(right_next_position, end_position) + 1;

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

                    stack[stack_index + index_offset1] = new Step(forward_next_position, forward_direction, heuristic1);
                    stack[stack_index + index_offset2] = new Step(left_next_position, left_direction, heuristic2);
                    stack[stack_index + index_offset3] = new Step(right_next_position, right_direction, heuristic3);

                    stack_index += 3;
                }
            }
        }

        return (found, parents);
    }
}
