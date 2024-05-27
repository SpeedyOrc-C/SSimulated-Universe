namespace SSimulated_Universe.Utility.Modifiable.Set;

public class ModifierSet<T>
{
    public readonly HashSet<T> Elements;
    
    private readonly HashSet<ModifiableSet<T>> _modifiables = new();

    public ModifierSet(HashSet<T> elements)
    {
        Elements = elements;
    }

    public void Modify(ModifiableSet<T> m)
    {
        _modifiables.Add(m);
        m.AddModifier(this);
    }

    public void Dismiss(ModifiableSet<T> m)
    {
        _modifiables.Remove(m);
        m.RemoveModifier(this);
    }

    public void DismissAll()
    {
        foreach (var m in _modifiables) 
            Dismiss(m);
        
        _modifiables.Clear();
    }
}
