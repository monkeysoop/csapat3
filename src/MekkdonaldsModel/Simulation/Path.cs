namespace Mekkdonalds.Simulation;

public class Path(List<Action> lista, Point p)
{
    private readonly List<Action> l = [.. lista];

    private int _ind;

    public readonly Point Target = p;

    public bool IsOver => _ind >= l.Count;

    public IReadOnlyList<Action> PlannedPath => l.Take(_ind).ToList();

    internal Action? this[int i]
    {
        get => i >= l.Count || i < 0 ? null : l[i];
    }

    internal Action Next()
    {
        if (_ind >= l.Count) throw new InvalidOperationException("No more actions in path");

        return l[_ind++];
    }
}
