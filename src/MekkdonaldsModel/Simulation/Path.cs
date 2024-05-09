namespace Mekkdonalds.Simulation;

public class Path(IEnumerable<Action> path, Point? target)
{
    private readonly List<Action> _path = [.. path];

    private int _ind = 0;

    public readonly Point? Target = target;

    public bool IsOver => _ind >= _path.Count;

    public IReadOnlyList<Action> PlannedPath => _path.Take(_ind).ToList();

    public Action? this[int i]
    {
        get => i >= _path.Count || i < 0 ? null : _path[i];
    }

    internal Action PeekNext()
    {
        if (_ind >= _path.Count)
        {
            throw new System.Exception("No more actions in path");
        }

        return _path[_ind];
    }

    internal void Increment()
    {
        _ind++;
    }

    public Action Next()
    {
        if (_ind >= _path.Count)
        {
            throw new System.Exception("No more actions in path");
        }

        return _path[_ind++];
    }

    internal bool FreeAllReserved(Board board, Point current_position, Direction currentDirection, int current_cost)
    {
        // this does not deal with timeout (Action.T) even if it is "planned"
        Point position = current_position;
        Direction direction = currentDirection;
        int cost = current_cost;

        while (!IsOver)
        {
            Action action = Next();
            board.UnReserve(position, cost);

            switch (action)
            {
                case Action.F:
                    board.UnReserve(position, cost + 1);
                    position = direction.GetNewOffsetPoint(position);
                    break;
                case Action.R:
                    direction = direction.ClockWise();
                    break;
                case Action.C:
                    direction = direction.CounterClockWise();
                    break;
                default:
                    return false;
            }

            cost++;
        }

        // needs to remove the reservation at the end

        return true;
    }

    internal void Alter(Action t)
    {
        _path.Insert(_ind, t);
    }
}
