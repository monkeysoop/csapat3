namespace Mekkdonalds.Simulation;

internal class Path
{
    private readonly List<Action> l;

    int ind;

    public Path()
    {
        l = [];
    }

    internal Action? Next()
    {
        if (ind > l.Count) return null;

        return l[ind++];
    }
}
