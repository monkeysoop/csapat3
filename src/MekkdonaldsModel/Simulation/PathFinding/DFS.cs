namespace Mekkdonalds.Simulation.PathFinding;

public sealed class DFS : PathFinder
{
    protected override (bool, int[], int[]) FindPath(Board board, Point start_position, int start_direction, Point end_position, int start_cost)
    {
        // this depth first search uses heuristics to hopefully find a correct path quicker
        Step[] stack = new Step[5 * board.Height * board.Width];
        int stack_index = 0;
        int[] parents = new int[board.Height * board.Width]; // all items are automatically set to 0
        int[] costs = new int[board.Height * board.Width]; // all items are automatically set to 0


        if (board.SetSearchedIfEmptyStart(start_position, start_cost))
        {
            stack[stack_index] = new Step(start_position, start_direction, 0);
            stack_index++;

            costs[start_position.Y * board.Width + start_position.X] = start_cost;
            parents[start_position.Y * board.Width + start_position.X] = start_direction;
        }
        else
        {
            throw new System.Exception("start position is blocked by a WALL");
        }


        int backward_direction = (start_direction + 2) % 4;
        Point backward_offset = nexts_offsets[backward_direction];
        Point backward_next_position = new(start_position.X + backward_offset.X,
                                           start_position.Y + backward_offset.Y);
        int backward_cost = start_cost + 3;

        if (board.SetSearchedIfEmptyBackward(start_position, backward_next_position, backward_cost))
        {
            stack[stack_index] = new Step(backward_next_position, backward_direction, 0);
            stack_index++;

            costs[backward_next_position.Y * board.Width + backward_next_position.X] = backward_cost;
            parents[backward_next_position.Y * board.Width + backward_next_position.X] = backward_direction;
        }


        bool found = false;
        while (stack_index != 0 && !found)
        {
            stack_index--;
            Step current_step = stack[stack_index];
            if (board.Searchable(current_step.Position))
            {
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

                    int current_cost = costs[current_step.Position.Y * board.Width + current_step.Position.X];

                    int forward_cost = current_cost + 1;
                    int left_cost = current_cost + 2;
                    int right_cost = current_cost + 2;

                    //int heuristic1 = forward_cost +
                    //                 MaxTurnsRequired(forward_next_position, forward_offset, end_position) +
                    //                 ManhattenDistance(forward_next_position, end_position);
                    //
                    //int heuristic2 = left_cost +
                    //                 MaxTurnsRequired(left_next_position, left_offset, end_position) +
                    //                 ManhattenDistance(left_next_position, end_position);
                    //
                    //int heuristic3 = right_cost +
                    //                 MaxTurnsRequired(right_next_position, right_offset, end_position) +
                    //                 ManhattenDistance(right_next_position, end_position);


                    if (board.SetSearchedIfEmptyLeftRight(current_step.Position, left_next_position, left_cost))
                    {
                        stack[stack_index] = new Step(left_next_position, left_direction, 0);
                        stack_index++;

                        costs[left_next_position.Y * board.Width + left_next_position.X] = left_cost;
                        parents[left_next_position.Y * board.Width + left_next_position.X] = left_direction;
                    }
                    if (board.SetSearchedIfEmptyLeftRight(current_step.Position, right_next_position, right_cost))
                    {
                        stack[stack_index] = new Step(right_next_position, right_direction, 0);
                        stack_index++;

                        costs[right_next_position.Y * board.Width + right_next_position.X] = right_cost;
                        parents[right_next_position.Y * board.Width + right_next_position.X] = right_direction;
                    }
                    if (board.SetSearchedIfEmptyForward(forward_next_position, forward_cost))
                    {
                        stack[stack_index] = new Step(forward_next_position, forward_direction, 0);
                        stack_index++;

                        costs[forward_next_position.Y * board.Width + forward_next_position.X] = forward_cost;
                        parents[forward_next_position.Y * board.Width + forward_next_position.X] = forward_direction;
                    }


                    //int index_offset1 = 2;
                    //int index_offset2 = 1;
                    //int index_offset3 = 0;
                    //
                    //if (heuristic1 > heuristic2)
                    //{
                    //    int t = heuristic1;
                    //    heuristic1 = heuristic2;
                    //    heuristic2 = t;
                    //    index_offset1 = 1;
                    //    index_offset2 = 2;
                    //}
                    //if (heuristic1 > heuristic3)
                    //{
                    //    int t = heuristic1;
                    //    heuristic1 = heuristic3;
                    //    heuristic3 = t;
                    //    index_offset3 = index_offset1;
                    //    index_offset1 = 0;
                    //}
                    //if (heuristic2 > heuristic3)
                    //{
                    //    int t = heuristic2;
                    //    heuristic2 = heuristic3;
                    //    heuristic3 = t;
                    //    int tt = index_offset2;
                    //    index_offset2 = index_offset3;
                    //    index_offset3 = tt;
                    //}
                    //
                    //stack[stack_index + index_offset1] = new Step(forward_next_position, forward_direction, heuristic1);
                    //stack[stack_index + index_offset2] = new Step(left_next_position, left_direction, heuristic2);
                    //stack[stack_index + index_offset3] = new Step(right_next_position, right_direction, heuristic3);
                    //
                    //stack_index += 3;
                }
            }
        }

        return (found, parents, costs);
    }
}