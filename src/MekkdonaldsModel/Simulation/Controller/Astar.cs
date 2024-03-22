namespace Mekkdonalds.Simulation.Controller;

public sealed class AstarController: SimulationController
{
    protected override (bool, int[]) FindPath(Board2 board, Point start_position, int start_direction, Point end_position)
    {
        const int COST_BIAS = 1;
        const int HEURISTIC_BIAS = 1;

        Step[] heap = new Step[5 * board.Height * board.Width];


        int heap_length = 0;
        HeapInsert(heap, heap_length, new Step(start_position, start_direction, 0));
        heap_length++;


        int[] costs = new int[board.Height * board.Width]; // all items are automatically set to 0
        int[] parents = new int[board.Height * board.Width]; // all items are automatically set to 0
        //for (int i = 0; i < board.height * board.width; i++)
        //{
        //    parents[i] = NO_PARENT;
        //} 

        parents[start_position.Y * board.Width + start_position.X] = start_direction;

        bool found = false;
        while (heap_length != 0 && !found)
        {
            Step current_step = HeapRemoveMin(heap, heap_length);
            heap_length--;

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

                int forward_heuristic = COST_BIAS * forward_cost +
                                        HEURISTIC_BIAS * MaxTurnsRequired(forward_next_position, forward_offset, end_position) +
                                        HEURISTIC_BIAS * ManhattenDistance(forward_next_position, end_position);

                int left_heuristic = COST_BIAS * left_cost +
                                     HEURISTIC_BIAS * MaxTurnsRequired(left_next_position, left_offset, end_position) +
                                     HEURISTIC_BIAS * ManhattenDistance(left_next_position, end_position);

                int right_heuristic = COST_BIAS * right_cost +
                                      HEURISTIC_BIAS * MaxTurnsRequired(right_next_position, right_offset, end_position) +
                                      HEURISTIC_BIAS * ManhattenDistance(right_next_position, end_position);


                if (board.SetSearchedIfEmpty(forward_next_position))
                {
                    costs[forward_next_position.Y * board.Width + forward_next_position.X] = forward_cost;
                    parents[forward_next_position.Y * board.Width + forward_next_position.X] = forward_direction;
                    HeapInsert(heap, heap_length, new Step(forward_next_position, forward_direction, forward_heuristic));
                    heap_length++;
                }
                else if (forward_cost < costs[forward_next_position.Y * board.Width + forward_next_position.X])
                {
                    costs[forward_next_position.Y * board.Width + forward_next_position.X] = forward_cost;
                    parents[forward_next_position.Y * board.Width + forward_next_position.X] = forward_direction;
                }


                if (board.SetSearchedIfEmpty(left_next_position))
                {
                    costs[left_next_position.Y * board.Width + left_next_position.X] = left_cost;
                    parents[left_next_position.Y * board.Width + left_next_position.X] = left_direction;
                    HeapInsert(heap, heap_length, new Step(left_next_position, left_direction, left_heuristic));
                    heap_length++;
                }
                else if (left_cost < costs[left_next_position.Y * board.Width + left_next_position.X])
                {
                    costs[left_next_position.Y * board.Width + left_next_position.X] = left_cost;
                    parents[left_next_position.Y * board.Width + left_next_position.X] = left_direction;
                }


                if (board.SetSearchedIfEmpty(right_next_position))
                {
                    costs[right_next_position.Y * board.Width + right_next_position.X] = right_cost;
                    parents[right_next_position.Y * board.Width + right_next_position.X] = right_direction;
                    HeapInsert(heap, heap_length, new Step(right_next_position, right_direction, right_heuristic));
                    heap_length++;
                }
                else if (right_cost < costs[right_next_position.Y * board.Width + right_next_position.X])
                {
                    costs[right_next_position.Y * board.Width + right_next_position.X] = right_cost;
                    parents[right_next_position.Y * board.Width + right_next_position.X] = right_direction;
                }
            }
        }
        return (found, parents);
    }

    private static void HeapInsert(Step[] heap, int length, Step item)
    {
        heap[length] = item;

        int index = length;
        int root_index = (index - 1) / 2;


        // <= is used (instead of <), because it makes the newer Step with same value prefered, 
        // which usually leads to finding the solution quicker.
        while (index > 0 && heap[index].Heuristic <= heap[root_index].Heuristic)
        {
            // swap
            (heap[root_index], heap[index]) = (heap[index], heap[root_index]);
            index = root_index;
            root_index = (index - 1) / 2;
        }
    }

    private static Step HeapRemoveMin(Step[] heap, int length)
    {
        Step min_item = heap[0];
        heap[0] = heap[^1];
        length--; // this is only local!!!

        int index = 0;
        int left_child_index = 1;
        int right_child_index = 1;

        int next_child_index = left_child_index;
        if (right_child_index < length && heap[right_child_index].Heuristic < heap[left_child_index].Heuristic)
        {
            next_child_index = right_child_index;
        }

        while (left_child_index < length && heap[next_child_index].Heuristic < heap[index].Heuristic)
        {
            // swap
            (heap[next_child_index], heap[index]) = (heap[index], heap[next_child_index]);
            index = next_child_index;
            left_child_index = 2 * index + 1;
            right_child_index = 2 * index + 2;

            next_child_index = left_child_index;
            if (right_child_index < length && heap[right_child_index].Heuristic < heap[left_child_index].Heuristic)
            {
                next_child_index = right_child_index;
            }
        }

        return min_item;
    }

    
}

