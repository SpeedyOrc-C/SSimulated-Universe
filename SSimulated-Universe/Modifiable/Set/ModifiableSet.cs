namespace SSimulated_Universe.Modifiable.Set;

public class ModifiableSet<T>
{
    public HashSet<T> Base;

    public HashSet<T> Eval => _evaluate();
    
    private readonly HashSet<ModifierSet<T>> _modifiers = new();

    public ModifiableSet(HashSet<T> @base) => Base = @base;

    private HashSet<T> _evaluate()
    {
        var result = new HashSet<T>(Base);
        
        foreach (var m in _modifiers)
            result.UnionWith(m.Elements);
        
        return result;
    }

    public void AddModifier(ModifierSet<T> m)
    {
        _modifiers.Add(m);
    }

    public void RemoveModifier(ModifierSet<T> m)
    {
        _modifiers.Remove(m);
    }
}
