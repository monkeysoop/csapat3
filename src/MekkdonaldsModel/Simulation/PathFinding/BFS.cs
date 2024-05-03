namespace Mekkdonalds.Simulation.PathFinding;

public sealed class BFS : PathFinder
{
    protected override (bool, int[], int[]) FindPath(Board board, Point start_position, int start_direction, Point end_position, int start_cost)
    {
        Step[] heap = new Step[5 * board.Height * board.Width];
        int heap_length = 0;

        int[] heap_hashmap = new int[board.Height * board.Width];

        int[] costs = new int[board.Height * board.Width]; // all items are automatically set to 0
        int[] parents = new int[board.Height * board.Width]; // all items are automatically set to 0

        for (int i = 0; i < board.Height * board.Width; i++)
        {
            heap_hashmap[i] = -1;
        }


        if (board.SetSearchedIfEmptyStart(start_position, start_cost))
        {
            //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
            HeapInsert(heap, heap_length, new Step(start_position, start_direction, start_cost), heap_hashmap, board.Width);
            heap_length++;
            //CheckHeap(heap, heap_length, heap_hashmap, board.Width);

            costs[start_position.Y * board.Width + start_position.X] = start_cost;
            parents[start_position.Y * board.Width + start_position.X] = start_direction;
        }
        else
        {
            throw new System.Exception("start position is blocked by a WALL");
        }


        int backward_direction = (start_direction + 2) % 4;
        Point backward_offset = nextOffsets[backward_direction];
        Point backward_next_position = new(start_position.X + backward_offset.X,
                                           start_position.Y + backward_offset.Y);
        int backward_cost = start_cost + 3;
        int backward_heuristic = backward_cost +
                                 MaxTurnsRequired(backward_next_position, backward_offset, end_position) +
                                 ManhattanDistance(backward_next_position, end_position);

        if (board.SetSearchedIfEmptyBackward(start_position, backward_next_position, backward_cost))
        {
            //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
            HeapInsert(heap, heap_length, new Step(backward_next_position, backward_direction, backward_heuristic), heap_hashmap, board.Width);
            heap_length++;
            //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
            costs[backward_next_position.Y * board.Width + backward_next_position.X] = backward_cost;
            parents[backward_next_position.Y * board.Width + backward_next_position.X] = backward_direction;
        }


        bool found = false;
        //while (heap_length != 0 && !found)
        while (heap_length != 0 && !found)
        {
            //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
            Step current_step = HeapRemoveMin(heap, heap_length, heap_hashmap, board.Width);
            heap_length--;
            //CheckHeap(heap, heap_length, heap_hashmap, board.Width);

            if (ComparePoints(current_step.Position, end_position))
            {
                found = true;
            }
            else
            {
                int forward_direction = current_step.Direction;
                int left_direction = (current_step.Direction + 3) % 4;
                int right_direction = (current_step.Direction + 1) % 4;

                Point forward_offset = nextOffsets[forward_direction];
                Point left_offset = nextOffsets[left_direction];
                Point right_offset = nextOffsets[right_direction];

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

                int forward_heuristic = forward_cost;
                int left_heuristic = left_cost;
                int right_heuristic = right_cost;


                if (board.SetSearchedIfEmptyForward(forward_next_position, forward_cost))
                {
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    HeapInsert(heap, heap_length, new Step(forward_next_position, forward_direction, forward_heuristic), heap_hashmap, board.Width);
                    heap_length++;
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    costs[forward_next_position.Y * board.Width + forward_next_position.X] = forward_cost;
                    parents[forward_next_position.Y * board.Width + forward_next_position.X] = forward_direction;
                }
                else if ((forward_cost < costs[forward_next_position.Y * board.Width + forward_next_position.X]) &&
                        (board.NotReservedForward(forward_next_position, forward_cost)) &&
                        (heap_hashmap[forward_next_position.Y * board.Width + forward_next_position.X] != -1))
                {
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    UpdateHeapItem(heap, heap_length, new Step(forward_next_position, forward_direction, forward_heuristic), heap_hashmap, board.Width);
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    costs[forward_next_position.Y * board.Width + forward_next_position.X] = forward_cost;
                    parents[forward_next_position.Y * board.Width + forward_next_position.X] = forward_direction;
                }



                if (board.SetSearchedIfEmptyLeftRight(current_step.Position, left_next_position, left_cost))
                {
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    HeapInsert(heap, heap_length, new Step(left_next_position, left_direction, left_heuristic), heap_hashmap, board.Width);
                    heap_length++;
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    costs[left_next_position.Y * board.Width + left_next_position.X] = left_cost;
                    parents[left_next_position.Y * board.Width + left_next_position.X] = left_direction;
                }
                else if ((left_cost < costs[left_next_position.Y * board.Width + left_next_position.X]) &&
                        (board.NotReservedLeftRight(current_step.Position, left_next_position, left_cost)) &&
                        (heap_hashmap[left_next_position.Y * board.Width + left_next_position.X] != -1))
                {
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    UpdateHeapItem(heap, heap_length, new Step(left_next_position, left_direction, left_heuristic), heap_hashmap, board.Width);
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    costs[left_next_position.Y * board.Width + left_next_position.X] = left_cost;
                    parents[left_next_position.Y * board.Width + left_next_position.X] = left_direction;
                }



                if (board.SetSearchedIfEmptyLeftRight(current_step.Position, right_next_position, right_cost))
                {
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    HeapInsert(heap, heap_length, new Step(right_next_position, right_direction, right_heuristic), heap_hashmap, board.Width);
                    heap_length++;
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    costs[right_next_position.Y * board.Width + right_next_position.X] = right_cost;
                    parents[right_next_position.Y * board.Width + right_next_position.X] = right_direction;
                }
                else if ((right_cost < costs[right_next_position.Y * board.Width + right_next_position.X]) &&
                        (board.NotReservedLeftRight(current_step.Position, right_next_position, right_cost)) &&
                        (heap_hashmap[right_next_position.Y * board.Width + right_next_position.X] != -1))
                {
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    UpdateHeapItem(heap, heap_length, new Step(right_next_position, right_direction, right_heuristic), heap_hashmap, board.Width);
                    //CheckHeap(heap, heap_length, heap_hashmap, board.Width);
                    costs[right_next_position.Y * board.Width + right_next_position.X] = right_cost;
                    parents[right_next_position.Y * board.Width + right_next_position.X] = right_direction;
                }

            }
        }
        return (found, parents, costs);
    }
}
