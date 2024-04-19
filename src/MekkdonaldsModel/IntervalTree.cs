using IntervalTree;

namespace Mekkdonalds;

public class IntervalTree<TValue> : IntervalTree<int, TValue>
{
    public new void Add(int from, int to, TValue value)
    {
        base.Add(from, to - 1, value);
    }

    public new TValue? Query(int key) => Query(key, key).FirstOrDefault();

    public TValue? this[int key] => Query(key);

    public TValue this[int from, int to]
    {
        set
        {
            if (from < to)
                base.Add(from, to - 1, value);
            else if (from == to)
                base.Add(from, to, value);
            else
                throw new ArgumentException($"{nameof(from)} must be less than or equal to {nameof(to)}");
        }
    }
}