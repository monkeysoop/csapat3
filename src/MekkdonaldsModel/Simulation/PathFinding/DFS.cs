namespace Mekkdonalds.Simulation.PathFinding;

public sealed class DFS : PathFinder
{    
    protected override (bool, int[], int[]) FindPath(Board board, Point startPosition, int startDirection, Point endPosition, int startCost)
    {
        // this depth first search uses heuristics to hopefully find a correct path quicker
        Step[] stack = new Step[5 * board.Height * board.Width];
        int stackIndex = 0;
        int[] parents = new int[board.Height * board.Width];
        int[] costs = new int[board.Height * board.Width];


        if (!board.SetSearchedIfEmptyStart(startPosition, startCost))
        {
            throw new System.Exception("start position is blocked by a WALL");
        }

        stack[stackIndex] = new Step(startPosition, startDirection, 0);
        stackIndex++;

        costs[startPosition.Y * board.Width + startPosition.X] = startCost;
        parents[startPosition.Y * board.Width + startPosition.X] = startDirection;

        int backwardDirection = (startDirection + 2) % 4;
        Point backwardOffset = backwardDirection.GetOffset();
        Point backwardNextPosition = new(startPosition.X + backwardOffset.X,
                                           startPosition.Y + backwardOffset.Y);
        int backwardCost = startCost + 3;

        if (board.SetSearchedIfEmptyBackward(startPosition, backwardNextPosition, backwardCost))
        {
            stack[stackIndex] = new Step(backwardNextPosition, backwardDirection, 0);
            stackIndex++;

            costs[backwardNextPosition.Y * board.Width + backwardNextPosition.X] = backwardCost;
            parents[backwardNextPosition.Y * board.Width + backwardNextPosition.X] = backwardDirection;
        }


        bool found = false;
        while (stackIndex != 0 && !found)
        {
            stackIndex--;
            Step currentStep = stack[stackIndex];
            if (board.Searchable(currentStep.Position))
            {
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

                    //int heuristic1 = forwardCost +
                    //                 MaxTurnsRequired(forwardNextPosition, forwardOffset, endPosition) +
                    //                 ManhattenDistance(forwardNextPosition, endPosition);
                    //
                    //int heuristic2 = leftCost +
                    //                 MaxTurnsRequired(leftNextPosition, leftOffset, endPosition) +
                    //                 ManhattenDistance(leftNextPosition, endPosition);
                    //
                    //int heuristic3 = rightCost +
                    //                 MaxTurnsRequired(rightNextPosition, rightOffset, endPosition) +
                    //                 ManhattenDistance(rightNextPosition, endPosition);


                    if (board.SetSearchedIfEmptyLeftRight(currentStep.Position, leftNextPosition, leftCost))
                    {
                        stack[stackIndex] = new Step(leftNextPosition, leftDirection, 0);
                        stackIndex++;

                        costs[leftNextPosition.Y * board.Width + leftNextPosition.X] = leftCost;
                        parents[leftNextPosition.Y * board.Width + leftNextPosition.X] = leftDirection;
                    }
                    if (board.SetSearchedIfEmptyLeftRight(currentStep.Position, rightNextPosition, rightCost))
                    {
                        stack[stackIndex] = new Step(rightNextPosition, rightDirection, 0);
                        stackIndex++;

                        costs[rightNextPosition.Y * board.Width + rightNextPosition.X] = rightCost;
                        parents[rightNextPosition.Y * board.Width + rightNextPosition.X] = rightDirection;
                    }
                    if (board.SetSearchedIfEmptyForward(forwardNextPosition, forwardCost))
                    {
                        stack[stackIndex] = new Step(forwardNextPosition, forwardDirection, 0);
                        stackIndex++;

                        costs[forwardNextPosition.Y * board.Width + forwardNextPosition.X] = forwardCost;
                        parents[forwardNextPosition.Y * board.Width + forwardNextPosition.X] = forwardDirection;
                    }


                    //int indexOffset1 = 2;
                    //int indexOffset2 = 1;
                    //int indexOffset3 = 0;
                    //
                    //if (heuristic1 > heuristic2)
                    //{
                    //    int t = heuristic1;
                    //    heuristic1 = heuristic2;
                    //    heuristic2 = t;
                    //    indexOffset1 = 1;
                    //    indexOffset2 = 2;
                    //}
                    //if (heuristic1 > heuristic3)
                    //{
                    //    int t = heuristic1;
                    //    heuristic1 = heuristic3;
                    //    heuristic3 = t;
                    //    indexOffset3 = indexOffset1;
                    //    indexOffset1 = 0;
                    //}
                    //if (heuristic2 > heuristic3)
                    //{
                    //    int t = heuristic2;
                    //    heuristic2 = heuristic3;
                    //    heuristic3 = t;
                    //    int tt = indexOffset2;
                    //    indexOffset2 = indexOffset3;
                    //    indexOffset3 = tt;
                    //}
                    //
                    //stack[stackIndex + indexOffset1] = new Step(forwardNextPosition, forwardDirection, heuristic1);
                    //stack[stackIndex + indexOffset2] = new Step(leftNextPosition, leftDirection, heuristic2);
                    //stack[stackIndex + indexOffset3] = new Step(rightNextPosition, rightDirection, heuristic3);
                    //
                    //stackIndex += 3;
                }
            }
        }

        return (found, parents, costs);
    }
}