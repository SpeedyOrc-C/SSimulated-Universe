namespace SSimulated_Universe.Modifiable.Number;

public abstract class ModifierNumber<TModifiable>
{
    private readonly HashSet<TModifiable> _modifiables = new();

    protected abstract void Register(TModifiable modifiable);
    protected abstract void Cancel(TModifiable modifiable);

    /// <summary>
    /// Modify this value. Multiple calls to the same value are ignored.
    /// </summary>
    /// <param name="modifiable">Value being modified.</param>
    public void Modify(TModifiable modifiable)
    {
        _modifiables.Add(modifiable);
        Register(modifiable);
    }

    /// <summary>
    /// Stop modifying this value.
    /// </summary>
    /// <param name="modifiable">Value being dismissed.</param>
    public void Dismiss(TModifiable modifiable)
    {
        _modifiables.Remove(modifiable);
        Cancel(modifiable);
    }
    
    /// <summary>
    /// Stop modifying all values that have this modifier.
    /// This is very useful when an effect is about to disappear, therefore remove all the buffs applied.
    /// </summary>
    public void DismissAll()
    {
        foreach (var modifiable in _modifiables) 
            Dismiss(modifiable);
        
        _modifiables.Clear();
    }
}
