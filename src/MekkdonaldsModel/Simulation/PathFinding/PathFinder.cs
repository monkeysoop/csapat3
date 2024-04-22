namespace Mekkdonalds.Simulation.PathFinding;

internal abstract class PathFinder
{
    protected static readonly Point[] nexts_offsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    internal (bool, List<Action>) CalculatePath(Board board, Point start_position, int start_direction, Point end_position, int start_cost)
    {
        bool found;
        int[] parents_data;
        int[] costs_data;

        (found, parents_data, costs_data) = FindPath(board, start_position, start_direction, end_position, start_cost);
        board.ClearMask();

        if (found)
        {
            return (true, TracePath(parents_data, costs_data, board, start_position, end_position));
        }
        else
        {
            return (false, new List<Action>());
        }
    }    

    private static List<Action> TracePath(int[] parents_board, int[] costs_board, Board board, Point start, Point end)
    {
        List<Action> path = [];


        Point current_position = end;
        int current_direction = (parents_board[current_position.Y * board.Width + current_position.X] + 2) % 4;

        while (!ComparePoints(current_position, start))
        {
            Point next_offset = nexts_offsets[current_direction];

            Point next_position = new(current_position.X + next_offset.X,
                                      current_position.Y + next_offset.Y);

            int next_direction = (parents_board[next_position.Y * board.Width + next_position.X] + 2) % 4;

            int diff = current_direction - next_direction;


            path.Add(Action.F);
            switch (diff)
            {
                case -3:
                    path.Add(Action.R);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2); // for stopping robots clipping trough each other
                    break;
                case -2:
                    path.Add(Action.R);
                    path.Add(Action.R);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 3); // for stopping robots clipping trough each other
                    break;
                case -1:
                    path.Add(Action.C);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2); // for stopping robots clipping trough each other
                    break;
                case 0:
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1); // for stopping robots clipping trough each other
                    break;
                case 1:
                    path.Add(Action.R);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2); // for stopping robots clipping trough each other
                    break;
                case 2:
                    path.Add(Action.R);
                    path.Add(Action.R);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 3); // for stopping robots clipping trough each other
                    break;
                case 3:
                    path.Add(Action.C);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 1);
                    board.Reserve(current_position, costs_board[current_position.Y * board.Width + current_position.X]);
                    board.Reserve(next_position, costs_board[next_position.Y * board.Width + next_position.X] + 2); // for stopping robots clipping trough each other
                    break;
            }

            current_position = next_position;
            current_direction = next_direction;
        }
        board.Reserve(start, costs_board[start.Y * board.Width + start.X]);
        //board.Reserve(end, costs_board[end.Y * board.Width + end.X] + 1);

        path.Reverse();

        return path;
    }

    protected abstract (bool, int[], int[]) FindPath(Board board, Point start_position, int start_direction, Point end_position, int start_cost);

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

        if (diff_x == 0 && diff_y == 0)
        {
            return 0;
        } else if (dot_product * dot_product == (diff_x * diff_x + diff_y * diff_y) * 1) // note that direction is a unitvector so its length is 1
        {
            return 0;
        } else if (dot_product * dot_product == -1 * (diff_x * diff_x + diff_y * diff_y) * 1) // note that direction is a unitvector so its length is 1
        {
            return 2;
        } else if (dot_product > 0)
        {
            return 1;
        } else if (dot_product < 0)
        {
            return 2;
        } else if (dot_product == 0)
        {
            return 1;
        } else
        {
            throw new ArgumentException();
        }

        //if (dot_product > 0 || (diff_x == 0 && diff_y == 0))
        //{
        //    return 0;
        //} else if (dot_product * dot_product != -1 * (diff_x * diff_x + diff_y * diff_y) * 1) // note that direction is a unitvector so its length is 1
        //{
        //    return 1;
        //} else
        //{
        //    //Debug.WriteLine("position:" + position);
        //    //Debug.WriteLine("end:" + end);
        //    //Debug.WriteLine("direction:" + direction);
        //    //Debug.WriteLine("diff:" + new Point(diff_x, diff_y));
        //    //Debug.WriteLine("dot product:" + dot_product);
        //    return 2;
        //}
    }

    protected static void CheckHeap(Step[] heap, int length, int[] heap_hashmap, int width)
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

    protected static void HeapInsert(Step[] heap, int length, Step item, int[] heap_hashmap, int width)
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

    protected static Step HeapRemoveMin(Step[] heap, int length, int[] heap_hashmap, int width)
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

    protected static void UpdateHeapItem(Step[] heap, int length, Step item, int[] heap_hashmap, int width)
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
