#define NO_CHECK_HEAP

namespace Mekkdonalds.Simulation.PathFinding;

public abstract class PathFinder
{
    /// <summary>
    /// Finds a path from start and start direction to end position and reserves it
    /// </summary>
    /// <param name="board">The board where it searches</param>
    /// <param name="startPosition">Robot's starting position</param>
    /// <param name="startDirection">Robot's starting direction</param>
    /// <param name="endPosition">Target's position</param>
    /// <param name="startCost">Used for the the start time in the reservation table</param>
    /// <returns>Returns true and the path if it finds any otherwise false and an empty path</returns>
    public (bool, List<Action>) CalculatePath(Board board, Point startPosition, int startDirection, Point endPosition, int startCost)
    {
        bool found;
        int[] parentsData;
        int[] costsData;

        (found, parentsData, costsData) = FindPath(board, startPosition, startDirection, endPosition, startCost);
        board.ClearMask();

        if (found)
        {
            return (true, TracePath(parentsData, costsData, board, startPosition, endPosition));
        }
        else
        {
            return (false, []);
        }
    }

    /// <summary>
    /// Given 2 arrays representing the parents and the arrival time for each node it traces back and reserves the path from end to start
    /// </summary>
    /// <param name="parentsBoard">array representing the direction to the searched parent nodes</param>
    /// <param name="costsBoard">array representing the expected arrival time for each searched position</param>
    /// <param name="board">The board where it searched</param>
    /// <param name="start">Robot's starting position</param>
    /// <param name="end">Target's position</param>
    /// <returns>The array of the actions to take in the correct order (start -> end)</returns>
    private static List<Action> TracePath(int[] parentsBoard, int[] costsBoard, Board board, Point start, Point end)
    {
        List<Action> path = [];


        Point currentPosition = end;
        int currentDirection = (parentsBoard[currentPosition.Y * board.Width + currentPosition.X] + 2) % 4;

        while (currentPosition != start)
        {
            Point nextOffset = currentDirection.GetOffset();

            Point nextPosition = new(currentPosition.X + nextOffset.X,
                                      currentPosition.Y + nextOffset.Y);

            int nextDirection = (parentsBoard[nextPosition.Y * board.Width + nextPosition.X] + 2) % 4;

            int diff = currentDirection - nextDirection;


            path.Add(Action.F);
            switch (diff)
            {
                case -3:
                case 1:
                    path.Add(Action.R);
                    board.Reserve(nextPosition, costsBoard[nextPosition.Y * board.Width + nextPosition.X] + 1);
                    board.Reserve(currentPosition, costsBoard[currentPosition.Y * board.Width + currentPosition.X]);
                    board.Reserve(nextPosition, costsBoard[nextPosition.Y * board.Width + nextPosition.X] + 2); // for stopping robots clipping trough each other
                    break;
                case -2:
                case 2:
                    path.Add(Action.R);
                    path.Add(Action.R);
                    board.Reserve(nextPosition, costsBoard[nextPosition.Y * board.Width + nextPosition.X] + 1);
                    board.Reserve(nextPosition, costsBoard[nextPosition.Y * board.Width + nextPosition.X] + 2);
                    board.Reserve(currentPosition, costsBoard[currentPosition.Y * board.Width + currentPosition.X]);
                    board.Reserve(nextPosition, costsBoard[nextPosition.Y * board.Width + nextPosition.X] + 3); // for stopping robots clipping trough each other
                    break;
                case -1:
                case 3:
                    path.Add(Action.C);
                    board.Reserve(nextPosition, costsBoard[nextPosition.Y * board.Width + nextPosition.X] + 1);
                    board.Reserve(currentPosition, costsBoard[currentPosition.Y * board.Width + currentPosition.X]);
                    board.Reserve(nextPosition, costsBoard[nextPosition.Y * board.Width + nextPosition.X] + 2); // for stopping robots clipping trough each other
                    break;
                case 0:
                    board.Reserve(currentPosition, costsBoard[currentPosition.Y * board.Width + currentPosition.X]);
                    board.Reserve(nextPosition, costsBoard[nextPosition.Y * board.Width + nextPosition.X] + 1); // for stopping robots clipping trough each other
                    break;
            }

            currentPosition = nextPosition;
            currentDirection = nextDirection;
        }
        board.Reserve(start, costsBoard[start.Y * board.Width + start.X]);

        path.Reverse();

        return path;
    }
    
    /// <summary>
    /// Finds a path from start and start direction to end position
    /// </summary>
    /// <param name="board">The board where it searches</param>
    /// <param name="start_position">Robot's starting position</param>
    /// <param name="startDirection">Robot's starting direction</param>
    /// <param name="end_position">Target's position</param>
    /// <param name="start_cost">Used for the the start time in the reservation table</param>
    /// <returns>Returns true if it finds a path otherwise false and 2 arrays first for parents data and second for the expected arrival times </returns>
    protected abstract (bool, int[], int[]) FindPath(Board board, Point start_position, int startDirection, Point end_position, int start_cost);

    /// <summary>
    /// Returns the Manhattan distance between 2 points (the order of the 2 points doesn't matter)
    /// </summary>
    /// <param name="start">First point</param>
    /// <param name="end">Second point</param>
    /// <returns>the sum of the absolute differences between the 2 point's coordinates</returns>
    protected static int ManhattanDistance(Point start, Point end) => Math.Abs(start.X - end.X) + Math.Abs(start.Y - end.Y);

    /// <summary>
    /// Heuristic used for approximating the number of turn steps
    /// </summary>
    /// <param name="position">Robot's current position</param>
    /// <param name="direction">Robot's current direction</param>
    /// <param name="end">Target's position</param>
    /// <returns>The heuristic value</returns>
    protected static int MaxTurnsRequired(Point position, Point direction, Point end)
    {
        int diffX = end.X - position.X;
        int diffY = end.Y - position.Y;
        int dotProduct = diffX * direction.X + diffY * direction.Y;

        if (dotProduct > 0)
        {
            return 1;
        }
        else if (dotProduct < 0)
        {
            return 2;
        } 
        else if ((diffX == 0 && diffY == 0) || dotProduct * dotProduct == (diffX * diffX + diffY * diffY) * 1) // note that direction is a unit vector so its length is 1
        {
            return 0;
        }
        else if (dotProduct * dotProduct == -1 * (diffX * diffX + diffY * diffY) * 1) // note that direction is a unit vector so its length is 1
        {
            return 2;
        }
        else // dotProduct == 0
        {
            return 1;
        }
    }

    /// <summary>
    /// Checks if the heap up to the given length has the properties of a minimum heap, also checks if the heap hash map (maps 2D coordinates to their position in the heap)
    /// </summary>
    /// <param name="heap">Buffered array of the min heap</param>
    /// <param name="length">The number of items inside the buffer</param>
    /// <param name="heapHashMap">A hash map that maps 2d coordinates to their index inside the buffer</param>
    /// <param name="width">Used because heap and heap hash map is flattened 2D arrays</param>
    /// <exception cref="System.Exception">Thrown if the heap or the heap hash map is bad</exception>
    protected static void CheckHeap(Step[] heap, int length, int[] heapHashMap, int width)
    {
#if CHECK_HEAP
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

        for (int i = 0; i < heapHashMap.Length; i++)
        {
            int y = (int)i / width;
            int x = i % width;
            int index = heapHashMap[i];
        
            if (heapHashMap[i] != -1 && heap[index].Position != new Point(x, y))
            {
                throw new System.Exception("error in heap hash map!");
            }
        }
#endif
    }

    /// <summary>
    /// Inserts an item into the given minimum heap while keeping its properties
    /// </summary>
    /// <param name="heap">Buffered array of the min heap</param>
    /// <param name="length">The number of items inside the buffer</param>
    /// <param name="item">The item to be inserted</param>
    /// <param name="heapHashMap">A hash map that maps 2d coordinates to their index inside the buffer</param>
    /// <param name="width">Used because heap and heap hash map is flattened 2D arrays</param>
    protected static void HeapInsert(Step[] heap, int length, Step item, int[] heapHashMap, int width)
    {
        CheckHeap(heap, length, heapHashMap, width);
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
        CheckHeap(heap, length + 1, heapHashMap, width);
    }

    /// <summary>
    /// Removes an item from the minimum heap while keeping its properties
    /// </summary>
    /// <param name="heap">Buffered array of the min heap</param>
    /// <param name="length">The number of items inside the buffer</param>
    /// <param name="heapHashMap">A hash map that maps 2d coordinates to their index inside the buffer</param>
    /// <param name="width">Used because heap and heap hash map is flattened 2D arrays</param>
    /// <returns>The minimum item</returns>
    protected static Step HeapRemoveMin(Step[] heap, int length, int[] heapHashMap, int width)
    {
        CheckHeap(heap, length, heapHashMap, width);
        Step minItem = heap[0];
        heapHashMap[heap[0].Position.Y * width + heap[0].Position.X] = -1;

        heap[0] = heap[length - 1];
        if (length > 1) 
        {
            // it doesnt matter if heap stays the same if theres only 1 item (because heapLength), but setting heapHashMap matters
            heapHashMap[heap[length - 1].Position.Y * width + heap[length - 1].Position.X] = 0;
        }

        length--; // this is only local!!!

        int index = 0;
        int leftChildIndex = 1;
        int rightChildIndex = 2;

        int nextChildIndex = leftChildIndex;
        if (rightChildIndex < length && heap[rightChildIndex].Heuristic < heap[leftChildIndex].Heuristic)
        {
            nextChildIndex = rightChildIndex;
        }

        while (leftChildIndex < length && heap[nextChildIndex].Heuristic < heap[index].Heuristic)
        {
            int hashMapIndex = heap[index].Position.Y * width + heap[index].Position.X;
            int hashMapNextChildIndex = heap[nextChildIndex].Position.Y * width + heap[nextChildIndex].Position.X;

            // swap
            (heap[nextChildIndex], heap[index]) = (heap[index], heap[nextChildIndex]);
            (heapHashMap[hashMapNextChildIndex], heapHashMap[hashMapIndex]) = (heapHashMap[hashMapIndex], heapHashMap[hashMapNextChildIndex]);

            index = nextChildIndex;
            leftChildIndex = 2 * index + 1;
            rightChildIndex = 2 * index + 2;

            nextChildIndex = leftChildIndex;
            if (rightChildIndex < length && heap[rightChildIndex].Heuristic < heap[leftChildIndex].Heuristic)
            {
                nextChildIndex = rightChildIndex;
            }
        }

        CheckHeap(heap, length, heapHashMap, width); // length is decreased locally
        return minItem;
    }

    /// <summary>
    /// Updates the value of an item already inside the heap while keeping its properties
    /// </summary>
    /// <param name="heap">Buffered array of the min heap</param>
    /// <param name="length">The number of items inside the buffer</param>
    /// <param name="item">The item to be updated</param>
    /// <param name="heapHashMap">A hash map that maps 2d coordinates to their index inside the buffer</param>
    /// <param name="width">Used because heap and heap hash map is flattened 2D arrays</param>
    /// <exception cref="System.Exception">Thrown if the item (to be updated) is not inside the heap</exception>
    protected static void UpdateHeapItem(Step[] heap, int length, Step item, int[] heapHashMap, int width)
    {
        CheckHeap(heap, length, heapHashMap, width);
        int index = heapHashMap[item.Position.Y * width + item.Position.X];

#if CHECK_HEAP
        if (index < 0)
        {
            throw new System.Exception("invalid index, item not in heap");
        }
#endif

        if (heap[index].Heuristic < item.Heuristic)
        {
            // cascade up item
            heap[index] = item;

            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;

            int nextChildIndex = leftChildIndex;
            if (rightChildIndex < length && heap[rightChildIndex].Heuristic < heap[leftChildIndex].Heuristic)
            {
                nextChildIndex = rightChildIndex;
            }

            while (leftChildIndex < length && heap[nextChildIndex].Heuristic < heap[index].Heuristic)
            {
                int hashMapIndex = heap[index].Position.Y * width + heap[index].Position.X;
                int hashMapNextChildIndex = heap[nextChildIndex].Position.Y * width + heap[nextChildIndex].Position.X;

                // swap
                (heap[nextChildIndex], heap[index]) = (heap[index], heap[nextChildIndex]);
                (heapHashMap[hashMapNextChildIndex], heapHashMap[hashMapIndex]) = (heapHashMap[hashMapIndex], heapHashMap[hashMapNextChildIndex]);

                index = nextChildIndex;
                leftChildIndex = 2 * index + 1;
                rightChildIndex = 2 * index + 2;

                nextChildIndex = leftChildIndex;
                if (rightChildIndex < length && heap[rightChildIndex].Heuristic < heap[leftChildIndex].Heuristic)
                {
                    nextChildIndex = rightChildIndex;
                }
            }

        }
        else if (heap[index].Heuristic > item.Heuristic)
        {
            // cascade down item
            heap[index] = item;

            int rootIndex = (index - 1) / 2;

            // <= is used (instead of <), because it makes the newer Step with same value preferred, 
            // which usually leads to finding the solution quicker.
            while (index > 0 && heap[index].Heuristic <= heap[rootIndex].Heuristic)
            {
                int hashMapIndex = heap[index].Position.Y * width + heap[index].Position.X;
                int hashMapRootIndex = heap[rootIndex].Position.Y * width + heap[rootIndex].Position.X;

                // swap
                (heap[rootIndex], heap[index]) = (heap[index], heap[rootIndex]);
                (heapHashMap[hashMapRootIndex], heapHashMap[hashMapIndex]) = (heapHashMap[hashMapIndex], heapHashMap[hashMapRootIndex]);

                index = rootIndex;
                rootIndex = (index - 1) / 2;
            }
        }
        CheckHeap(heap, length, heapHashMap, width);
    }
}
