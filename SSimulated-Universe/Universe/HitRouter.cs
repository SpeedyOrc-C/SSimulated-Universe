namespace SSimulated_Universe.Universe;

public abstract class HitSplitter
{
    public abstract List<Hit> Split(Hit hit);
    public abstract int ComparePriority(HitSplitter other);
}

public class HitSplitterPriorityComparer : IComparer<HitSplitter>
{
    public int Compare(HitSplitter? x, HitSplitter? y)
    {
        if (x is null || y is null)
            throw new Exception("Cannot compare null");

        return x.ComparePriority(y);
    }
}
