namespace Mekkdonalds.Simulation.PathFinding;

public abstract class PathFinder
{
    protected static readonly Point[] nextOffsets = [
        new(0, -1),
        new(1, 0),
        new(0, 1),
        new(-1, 0)
    ];

    public (bool, List<Action>) CalculatePath(Board board, Point start_position, int start_direction, Point end_position, int start_cost)
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
            Point next_offset = nextOffsets[current_direction];

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

    protected static int ManhattanDistance(Point start, Point end)
    {
        return Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);
    }

    protected static int MaxTurnsRequired(Point position, Point direction, Point end)
    {
        int diffX = end.X - position.X;
        int diffY = end.Y - position.Y;
        int dotProduct = diffX * direction.X + diffY * direction.Y;

        if (diffX == 0 && diffY == 0)
        {
            return 0;
        }
        else if (dotProduct * dotProduct == (diffX * diffX + diffY * diffY) * 1) // note that direction is a unit vector so its length is 1
        {
            return 0;
        }
        else if (dotProduct * dotProduct == -1 * (diffX * diffX + diffY * diffY) * 1) // note that direction is a unit vector so its length is 1
        {
            return 2;
        }
        else if (dotProduct > 0)
        {
            return 1;
        }
        else if (dotProduct < 0)
        {
            return 2;
        }
        else if (dotProduct == 0)
        {
            return 1;
        }
        else
        {
            throw new ArgumentException();
        }
    }

    protected static void CheckHeap(Step[] heap, int length, int[] heapHashMap, int width)
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
            if (heapHashMap[heap[i].Position.Y * width + heap[i].Position.X] != i)
            {
                throw new System.Exception("error in heap hash map!");
            }
        }
    }

    protected static void HeapInsert(Step[] heap, int length, Step item, int[] heapHashMap, int width)
    {
        heap[length] = item;
        heapHashMap[item.Position.Y * width + item.Position.X] = length;

        int index = length;
        int root_index = (index - 1) / 2;


        // <= is used (instead of <), because it makes the newer Step with same value preferred, 
        // which usually leads to finding the solution quicker.
        while (index > 0 && heap[index].Heuristic <= heap[root_index].Heuristic)
        {
            int hashMapIndex = heap[index].Position.Y * width + heap[index].Position.X;
            int hashMapRootIndex = heap[root_index].Position.Y * width + heap[root_index].Position.X;

            // swap
            (heap[root_index], heap[index]) = (heap[index], heap[root_index]);
            (heapHashMap[hashMapRootIndex], heapHashMap[hashMapIndex]) = (heapHashMap[hashMapIndex], heapHashMap[hashMapRootIndex]);

            index = root_index;
            root_index = (index - 1) / 2;
        }
    }

    protected static Step HeapRemoveMin(Step[] heap, int length, int[] heapHashMap, int width)
    {
        Step minItem = heap[0];
        heapHashMap[heap[0].Position.Y * width + heap[0].Position.X] = -1;

        heap[0] = heap[length - 1];
        heapHashMap[heap[length - 1].Position.Y * width + heap[length - 1].Position.X] = 0;

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
            int hashMapIndex = heap[index].Position.Y * width + heap[index].Position.X;
            int hashMapNextChildIndex = heap[next_child_index].Position.Y * width + heap[next_child_index].Position.X;

            // swap
            (heap[next_child_index], heap[index]) = (heap[index], heap[next_child_index]);
            (heapHashMap[hashMapNextChildIndex], heapHashMap[hashMapIndex]) = (heapHashMap[hashMapIndex], heapHashMap[hashMapNextChildIndex]);

            index = next_child_index;
            left_child_index = 2 * index + 1;
            right_child_index = 2 * index + 2;

            next_child_index = left_child_index;
            if (right_child_index < length && heap[right_child_index].Heuristic < heap[left_child_index].Heuristic)
            {
                next_child_index = right_child_index;
            }
        }

        return minItem;
    }

    protected static void UpdateHeapItem(Step[] heap, int length, Step item, int[] heapHashMap, int width)
    {
        int index = heapHashMap[item.Position.Y * width + item.Position.X];

        if (index >= length || index < 0)
        {
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
                int hashMapIndex = heap[index].Position.Y * width + heap[index].Position.X;
                int hashMapNextChildIndex = heap[next_child_index].Position.Y * width + heap[next_child_index].Position.X;

                // swap
                (heap[next_child_index], heap[index]) = (heap[index], heap[next_child_index]);
                (heapHashMap[hashMapNextChildIndex], heapHashMap[hashMapIndex]) = (heapHashMap[hashMapIndex], heapHashMap[hashMapNextChildIndex]);

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

            // <= is used (instead of <), because it makes the newer Step with same value preferred, 
            // which usually leads to finding the solution quicker.
            while (index > 0 && heap[index].Heuristic <= heap[root_index].Heuristic)
            {
                int hashMapIndex = heap[index].Position.Y * width + heap[index].Position.X;
                int hashMapRootIndex = heap[root_index].Position.Y * width + heap[root_index].Position.X;

                // swap
                (heap[root_index], heap[index]) = (heap[index], heap[root_index]);
                (heapHashMap[hashMapRootIndex], heapHashMap[hashMapIndex]) = (heapHashMap[hashMapIndex], heapHashMap[hashMapRootIndex]);

                index = root_index;
                root_index = (index - 1) / 2;
            }
        }
    }
}
