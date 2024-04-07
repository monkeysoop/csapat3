using IntervalTree;

namespace Mekkdonalds;

public class IntervalTree<TValue> : IntervalTree<int, TValue>
{
    public new void Add(int from, int to, TValue value)
    {
        base.Add(from, to - 1, value);
    }

    public new TValue Query(int key) => Query(key, key).FirstOrDefault()!;

    public TValue this[int key] => Query(key);

    public TValue this[int from, int to]
    {
        set
        {
            base.Add(from, to - 1, value);
        }
    }
}