namespace Mekkdonalds.Simulation;

public class Path(List<Action> lista, Point p)
{
    public readonly Point Target = p;

    private readonly List<Action> l = [.. lista];

    int _ind;

    internal Action? this[int i]
    {
        get => i >= l.Count || i < 0 ? null : l[i];
    }

    internal Action? Next()
    {
        if (_ind >= l.Count) return null;

        return l[_ind++];
    }
}
