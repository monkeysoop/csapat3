using IntervalTree;

namespace Mekkdonalds;

public class IntervalTree<TValue> : IntervalTree<uint, TValue>
{
    public void Add(int from, int to, TValue value)
    {
        if (from < 0 || to < 0)
            throw new ArgumentException($"{nameof(from)} and {nameof(to)} must be non-negative");

        if (from < to)
            base.Add((uint)from, (uint)to - 1, value);
        else if (from == to)
            base.Add((uint)from, (uint)to, value);
        else
            throw new ArgumentException($"{nameof(from)} must be less than or equal to {nameof(to)}");
    }

    public new TValue? Query(int key)
    {
        if (key < 0)
            throw new ArgumentException($"{nameof(key)} must be non-negative");

        return Query((uint)key, (uint)key).FirstOrDefault();
    }

    public TValue? this[int key] => Query(key);

    public TValue this[int from, int to]
    {
        set
        {
            Add(from, to, value);
        }
    }
}