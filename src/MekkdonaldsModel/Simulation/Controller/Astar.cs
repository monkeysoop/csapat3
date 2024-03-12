namespace Mekkdonalds.Simulation.Controller;


internal sealed class AstarController(double interval) : SimulationController(interval)
{
    //private const int UP_DIR = 0;
    //private const int RIGHT_DIR = 1;
    //private const int DOWN_DIR = 2;
    //private const int LEFT_DIR = 3;
    //private const int NO_PARENT = 4;

    private const int COST_BIAS = 1;
    private const int HEURISTIC_BIAS = 1;

    public AstarController() : this(1) { }

    private static void HeapInsert(Step[] heap, int length, Step item)
    {
        heap[length] = item;

        int index = length;
        int root_index = (index - 1) / 2;


        // <= is used (instead of <), because it makes the newer Step with same value prefered, 
        // which usually leads to finding the solution quicker.
        while (index > 0 && heap[index].heuristic <= heap[root_index].heuristic)
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
        if (right_child_index < length && heap[right_child_index].heuristic < heap[left_child_index].heuristic)
        {
            next_child_index = right_child_index;
        }

        while (left_child_index < length && heap[next_child_index].heuristic < heap[index].heuristic)
        {
            // swap
            (heap[next_child_index], heap[index]) = (heap[index], heap[next_child_index]);
            index = next_child_index;
            left_child_index = 2 * index + 1;
            right_child_index = 2 * index + 2;

            next_child_index = left_child_index;
            if (right_child_index < length && heap[right_child_index].heuristic < heap[left_child_index].heuristic)
            {
                next_child_index = right_child_index;
            }
        }

        return min_item;
    }

    private bool AstarPathFinder(Point start, int start_direction, Point end)
    {
        Step[] heap = new Step[5 * _board.Height * _board.Width];


        int heap_length = 0;
        HeapInsert(heap, heap_length, new Step(start, start_direction, 0));
        heap_length++;


        int[] costs = new int[_board.Height * _board.Width]; // all items are automatically set to 0
        int[] parents = new int[_board.Height * _board.Width]; // all items are automatically set to 0
        //for (int i = 0; i < board.height * board.width; i++)
        //{
        //    parents[i] = NO_PARENT;
        //} 


        bool found = false;
        while (heap_length != 0 && !found)
        {
            Step current_step = HeapRemoveMin(heap, heap_length);
            heap_length--;
            _board.SetSearched(current_step.position);

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

                int current_cost = costs[current_step.position.Y * _board.Width + current_step.position.X];

                int forward_cost = current_cost + 1;
                int left_cost = current_cost + 2;
                int right_cost = current_cost + 2;

                int forward_heuristic = COST_BIAS * forward_cost +
                                        HEURISTIC_BIAS * MaxTurnsRequired(forward_next_position, forward_offset, end) +
                                        HEURISTIC_BIAS * ManhattenDistance(forward_next_position, end);

                int left_heuristic = COST_BIAS * left_cost +
                                     HEURISTIC_BIAS * MaxTurnsRequired(left_next_position, left_offset, end) +
                                     HEURISTIC_BIAS * ManhattenDistance(left_next_position, end);

                int right_heuristic = COST_BIAS * right_cost +
                                      HEURISTIC_BIAS * MaxTurnsRequired(right_next_position, right_offset, end) +
                                      HEURISTIC_BIAS * ManhattenDistance(right_next_position, end);


                if (_board.SetOpenIfEmpty(forward_next_position))
                {
                    costs[forward_next_position.Y * _board.Width + forward_next_position.X] = forward_cost;
                    parents[forward_next_position.Y * _board.Width + forward_next_position.X] = forward_direction;
                    HeapInsert(heap, heap_length, new Step(forward_next_position, forward_direction, forward_heuristic));
                    heap_length++;
                }
                else if (forward_cost < costs[forward_next_position.Y * _board.Width + forward_next_position.X])
                {
                    costs[forward_next_position.Y * _board.Width + forward_next_position.X] = forward_cost;
                    parents[forward_next_position.Y * _board.Width + forward_next_position.X] = forward_direction;
                }


                if (_board.SetOpenIfEmpty(left_next_position))
                {
                    costs[left_next_position.Y * _board.Width + left_next_position.X] = left_cost;
                    parents[left_next_position.Y * _board.Width + left_next_position.X] = left_direction;
                    HeapInsert(heap, heap_length, new Step(left_next_position, left_direction, left_heuristic));
                    heap_length++;
                }
                else if (left_cost < costs[left_next_position.Y * _board.Width + left_next_position.X])
                {
                    costs[left_next_position.Y * _board.Width + left_next_position.X] = left_cost;
                    parents[left_next_position.Y * _board.Width + left_next_position.X] = left_direction;
                }


                if (_board.SetOpenIfEmpty(right_next_position))
                {
                    costs[right_next_position.Y * _board.Width + right_next_position.X] = right_cost;
                    parents[right_next_position.Y * _board.Width + right_next_position.X] = right_direction;
                    HeapInsert(heap, heap_length, new Step(right_next_position, right_direction, right_heuristic));
                    heap_length++;
                }
                else if (right_cost < costs[right_next_position.Y * _board.Width + right_next_position.X])
                {
                    costs[right_next_position.Y * _board.Width + right_next_position.X] = right_cost;
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

