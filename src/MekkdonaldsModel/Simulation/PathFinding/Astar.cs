namespace Mekkdonalds.Simulation.PathFinding;

public sealed class Astar : PathFinder
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
        } else
        {
            throw new System.Exception("start position is blocked by a WALL");
        }


        int backward_direction = (start_direction + 2) % 4;
        Point backward_offset = nexts_offsets[backward_direction];
        Point backward_next_position = new(start_position.X + backward_offset.X,
                                           start_position.Y + backward_offset.Y);
        int backward_cost = start_cost + 3;
        int backward_heuristic = backward_cost +
                                 MaxTurnsRequired(backward_next_position, backward_offset, end_position) +
                                 ManhattenDistance(backward_next_position, end_position);

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

                int forward_heuristic = forward_cost +
                                        MaxTurnsRequired(forward_next_position, forward_offset, end_position) +
                                        ManhattenDistance(forward_next_position, end_position);

                int left_heuristic = left_cost +
                                     MaxTurnsRequired(left_next_position, left_offset, end_position) +
                                     ManhattenDistance(left_next_position, end_position);

                int right_heuristic = right_cost +
                                      MaxTurnsRequired(right_next_position, right_offset, end_position) +
                                      ManhattenDistance(right_next_position, end_position);


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



    private static void CheckHeap(Step[] heap, int length, int[] heap_hashmap, int width)
    {
        for (int i = 1; i < length; i++)
        {
            int root_index = (i - 1) / 2;
            if (heap[root_index].Heuristic > heap[i].Heuristic)
            {
                throw new System.Exception("error in heap!");
            }
        }

        for (int i = 0; i < length; i++)
        {
            if (heap_hashmap[heap[i].Position.Y * width + heap[i].Position.X] != i)
            {
                //for (int j = 0; j < length; j++)
                //{
                //    Debug.WriteLine("j: " + j + "\t" + heap[j].Position + "\t" + heap[j].Direction + "\t" + heap_hashmap[heap[j].Position.Y * width + heap[j].Position.X]);
                //}
                throw new System.Exception("error in heap hashmap!");
            }
        }
    }

    private static void HeapInsert(Step[] heap, int length, Step item, int[] heap_hashmap, int width)
    {
        heap[length] = item;
        heap_hashmap[item.Position.Y * width + item.Position.X] = length;

        int index = length;
        int root_index = (index - 1) / 2;


        // <= is used (instead of <), because it makes the newer Step with same value prefered, 
        // which usually leads to finding the solution quicker.
        while (index > 0 && heap[index].Heuristic <= heap[root_index].Heuristic)
        {
            int hashmap_index = heap[index].Position.Y * width + heap[index].Position.X;
            int hashmap_root_index = heap[root_index].Position.Y * width + heap[root_index].Position.X;

            // swap
            (heap[root_index], heap[index]) = (heap[index], heap[root_index]);
            (heap_hashmap[hashmap_root_index], heap_hashmap[hashmap_index]) = (heap_hashmap[hashmap_index], heap_hashmap[hashmap_root_index]);

            index = root_index;
            root_index = (index - 1) / 2;
        }
    }

    private static Step HeapRemoveMin(Step[] heap, int length, int[] heap_hashmap, int width)
    {
        Step min_item = heap[0];
        heap_hashmap[heap[0].Position.Y * width + heap[0].Position.X] = -1;

        heap[0] = heap[length - 1];
        heap_hashmap[heap[length - 1].Position.Y * width + heap[length - 1].Position.X] = 0;

        length--; // this is only local!!!

        int index = 0;
        int left_child_index = 1;
        int right_child_index = 2;

        int next_child_index = left_child_index;
        if (right_child_index < length && heap[right_child_index].Heuristic < heap[left_child_index].Heuristic)
        {
            next_child_index = right_child_index;
        }

        while (left_child_index < length && heap[next_child_index].Heuristic < heap[index].Heuristic)
        {
            int hashmap_index = heap[index].Position.Y * width + heap[index].Position.X;
            int hashmap_next_child_index = heap[next_child_index].Position.Y * width + heap[next_child_index].Position.X;

            // swap
            (heap[next_child_index], heap[index]) = (heap[index], heap[next_child_index]);
            (heap_hashmap[hashmap_next_child_index], heap_hashmap[hashmap_index]) = (heap_hashmap[hashmap_index], heap_hashmap[hashmap_next_child_index]);

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

    private static void UpdateHeapItem(Step[] heap, int length, Step item, int[] heap_hashmap, int width)
    {
        int index = heap_hashmap[item.Position.Y * width + item.Position.X];
        if (index >= length || index < 0)
        {
            //Debug.WriteLine("index: " + index);
            //for (int i = 0; i < length; i++)
            //{
            //    Debug.WriteLine("i: " + i + "\t" + heap[i].Position + "\t" + heap[i].Direction + "\t" + heap_hashmap[heap[i].Position.Y * width + heap[i].Position.X]);
            //}
            throw new System.Exception("invalid index, item not in heap");
        }
        if (heap[index].Heuristic < item.Heuristic)
        {
            // cascade up item
            heap[index] = item;

            int left_child_index = 2 * index + 1;
            int right_child_index = 2 * index + 2;

            int next_child_index = left_child_index;
            if (right_child_index < length && heap[right_child_index].Heuristic < heap[left_child_index].Heuristic)
            {
                next_child_index = right_child_index;
            }

            while (left_child_index < length && heap[next_child_index].Heuristic < heap[index].Heuristic)
            {
                int hashmap_index = heap[index].Position.Y * width + heap[index].Position.X;
                int hashmap_next_child_index = heap[next_child_index].Position.Y * width + heap[next_child_index].Position.X;

                // swap
                (heap[next_child_index], heap[index]) = (heap[index], heap[next_child_index]);
                (heap_hashmap[hashmap_next_child_index], heap_hashmap[hashmap_index]) = (heap_hashmap[hashmap_index], heap_hashmap[hashmap_next_child_index]);

                index = next_child_index;
                left_child_index = 2 * index + 1;
                right_child_index = 2 * index + 2;

                next_child_index = left_child_index;
                if (right_child_index < length && heap[right_child_index].Heuristic < heap[left_child_index].Heuristic)
                {
                    next_child_index = right_child_index;
                }
            }

        }
        else if (heap[index].Heuristic > item.Heuristic)
        {
            // cascade down item
            heap[index] = item;

            int root_index = (index - 1) / 2;

            // <= is used (instead of <), because it makes the newer Step with same value prefered, 
            // which usually leads to finding the solution quicker.
            while (index > 0 && heap[index].Heuristic <= heap[root_index].Heuristic)
            {
                int hashmap_index = heap[index].Position.Y * width + heap[index].Position.X;
                int hashmap_root_index = heap[root_index].Position.Y * width + heap[root_index].Position.X;

                // swap
                (heap[root_index], heap[index]) = (heap[index], heap[root_index]);
                (heap_hashmap[hashmap_root_index], heap_hashmap[hashmap_index]) = (heap_hashmap[hashmap_index], heap_hashmap[hashmap_root_index]);

                index = root_index;
                root_index = (index - 1) / 2;
            }
        }
    }
}