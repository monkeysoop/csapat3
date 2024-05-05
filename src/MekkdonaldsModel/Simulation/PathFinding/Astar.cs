namespace Mekkdonalds.Simulation.PathFinding;

public sealed class AStar : PathFinder
{
    protected override (bool, int[], int[]) FindPath(Board board, Point startPosition, int startDirection, Point endPosition, int startCost)
    {
        Step[] heap = new Step[5 * board.Height * board.Width];
        int heapLength = 0;

        int[] heapHashMap = new int[board.Height * board.Width];

        int[] costs = new int[board.Height * board.Width];
        int[] parents = new int[board.Height * board.Width];

        for (int i = 0; i < board.Height * board.Width; i++)
        {
            heapHashMap[i] = -1;
        }


        if (!board.SetSearchedIfEmptyStart(startPosition, startCost))
        {
            throw new InvalidOperationException("start position is blocked by a WALL");
        }

        HeapInsert(heap, heapLength, new Step(startPosition, startDirection, startCost), heapHashMap, board.Width);
        heapLength++;

        costs[startPosition.Y * board.Width + startPosition.X] = startCost;
        parents[startPosition.Y * board.Width + startPosition.X] = startDirection;


        int backwardDirection = (startDirection + 2) % 4;
        Point backwardOffset = backwardDirection.GetOffset();
        Point backwardNextPosition = new(startPosition.X + backwardOffset.X,
                                           startPosition.Y + backwardOffset.Y);
        int backwardCost = startCost + 3;
        int backwardHeuristic = backwardCost +
                                 MaxTurnsRequired(backwardNextPosition, backwardOffset, endPosition) +
                                 ManhattanDistance(backwardNextPosition, endPosition);

        if (board.SetSearchedIfEmptyBackward(startPosition, backwardNextPosition, backwardCost))
        {
            HeapInsert(heap, heapLength, new Step(backwardNextPosition, backwardDirection, backwardHeuristic), heapHashMap, board.Width);
            heapLength++;
            costs[backwardNextPosition.Y * board.Width + backwardNextPosition.X] = backwardCost;
            parents[backwardNextPosition.Y * board.Width + backwardNextPosition.X] = backwardDirection;
        }


        bool found = false;

        while (heapLength != 0 && !found)
        {
            Step currentStep = HeapRemoveMin(heap, heapLength, heapHashMap, board.Width);
            heapLength--;

            if (currentStep.Position == endPosition)
            {
                found = true;
            }
            else
            {
                int forwardDirection = currentStep.Direction;
                int leftDirection = (currentStep.Direction + 3) % 4;
                int rightDirection = (currentStep.Direction + 1) % 4;

                Point forwardOffset = forwardDirection.GetOffset();
                Point leftOffset = leftDirection.GetOffset();
                Point rightOffset = rightDirection.GetOffset();

                Point forwardNextPosition = new(currentStep.Position.X + forwardOffset.X,
                                                  currentStep.Position.Y + forwardOffset.Y);
                Point leftNextPosition = new(currentStep.Position.X + leftOffset.X,
                                               currentStep.Position.Y + leftOffset.Y);
                Point rightNextPosition = new(currentStep.Position.X + rightOffset.X,
                                                currentStep.Position.Y + rightOffset.Y);

                int currentCost = costs[currentStep.Position.Y * board.Width + currentStep.Position.X];

                int forwardCost = currentCost + 1;
                int leftCost = currentCost + 2;
                int rightCost = currentCost + 2;

                int forwardHeuristic = forwardCost +
                                        MaxTurnsRequired(forwardNextPosition, forwardOffset, endPosition) +
                                        ManhattanDistance(forwardNextPosition, endPosition);

                int leftHeuristic = leftCost +
                                     MaxTurnsRequired(leftNextPosition, leftOffset, endPosition) +
                                     ManhattanDistance(leftNextPosition, endPosition);

                int rightHeuristic = rightCost +
                                      MaxTurnsRequired(rightNextPosition, rightOffset, endPosition) +
                                      ManhattanDistance(rightNextPosition, endPosition);


                if (board.SetSearchedIfEmptyForward(forwardNextPosition, forwardCost))
                {
                    HeapInsert(heap, heapLength, new Step(forwardNextPosition, forwardDirection, forwardHeuristic), heapHashMap, board.Width);
                    heapLength++;

                    costs[forwardNextPosition.Y * board.Width + forwardNextPosition.X] = forwardCost;
                    parents[forwardNextPosition.Y * board.Width + forwardNextPosition.X] = forwardDirection;
                }
                else if ((forwardCost < costs[forwardNextPosition.Y * board.Width + forwardNextPosition.X]) &&
                        (board.NotReservedForward(forwardNextPosition, forwardCost)) &&
                        (heapHashMap[forwardNextPosition.Y * board.Width + forwardNextPosition.X] != -1))
                {
                    UpdateHeapItem(heap, heapLength, new Step(forwardNextPosition, forwardDirection, forwardHeuristic), heapHashMap, board.Width);
                    
                    costs[forwardNextPosition.Y * board.Width + forwardNextPosition.X] = forwardCost;
                    parents[forwardNextPosition.Y * board.Width + forwardNextPosition.X] = forwardDirection;
                }



                if (board.SetSearchedIfEmptyLeftRight(currentStep.Position, leftNextPosition, leftCost))
                {
                    
                    HeapInsert(heap, heapLength, new Step(leftNextPosition, leftDirection, leftHeuristic), heapHashMap, board.Width);
                    heapLength++;
                    
                    costs[leftNextPosition.Y * board.Width + leftNextPosition.X] = leftCost;
                    parents[leftNextPosition.Y * board.Width + leftNextPosition.X] = leftDirection;
                }
                else if ((leftCost < costs[leftNextPosition.Y * board.Width + leftNextPosition.X]) &&
                        (board.NotReservedLeftRight(currentStep.Position, leftNextPosition, leftCost)) &&
                        (heapHashMap[leftNextPosition.Y * board.Width + leftNextPosition.X] != -1))
                {                    
                    UpdateHeapItem(heap, heapLength, new Step(leftNextPosition, leftDirection, leftHeuristic), heapHashMap, board.Width);
                    
                    costs[leftNextPosition.Y * board.Width + leftNextPosition.X] = leftCost;
                    parents[leftNextPosition.Y * board.Width + leftNextPosition.X] = leftDirection;
                }



                if (board.SetSearchedIfEmptyLeftRight(currentStep.Position, rightNextPosition, rightCost))
                {
                    HeapInsert(heap, heapLength, new Step(rightNextPosition, rightDirection, rightHeuristic), heapHashMap, board.Width);
                    heapLength++;

                    costs[rightNextPosition.Y * board.Width + rightNextPosition.X] = rightCost;
                    parents[rightNextPosition.Y * board.Width + rightNextPosition.X] = rightDirection;
                }
                else if ((rightCost < costs[rightNextPosition.Y * board.Width + rightNextPosition.X]) &&
                        (board.NotReservedLeftRight(currentStep.Position, rightNextPosition, rightCost)) &&
                        (heapHashMap[rightNextPosition.Y * board.Width + rightNextPosition.X] != -1))
                {                    
                    UpdateHeapItem(heap, heapLength, new Step(rightNextPosition, rightDirection, rightHeuristic), heapHashMap, board.Width);
                    
                    costs[rightNextPosition.Y * board.Width + rightNextPosition.X] = rightCost;
                    parents[rightNextPosition.Y * board.Width + rightNextPosition.X] = rightDirection;
                }

            }
        }
        return (found, parents, costs);
    }
}