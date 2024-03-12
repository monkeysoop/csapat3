namespace Mekkdonalds.Simulation;

public class Path
{
    private readonly List<Action> l;

    int ind;

    public Path()
    {
        l = [];
    }

    public Path(List<Action> lista)
    {
        l = [.. lista];
    }

    public Action? this[int i]
    {
        get { return i >= l.Count || i < 0 ? null : l[i]; }
    }

    internal Action? Next()
    {
        if (ind >= l.Count) return null;

        return l[ind++];
    }
}
