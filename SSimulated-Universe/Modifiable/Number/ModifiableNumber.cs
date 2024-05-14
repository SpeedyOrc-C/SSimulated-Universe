using System.Numerics;

namespace SSimulated_Universe.Modifiable.Number;

public abstract class ModifiableNumber<T, ModifierImmediate, ModifierProportion>
    where T : INumber<T>
{
    /// <summary>
    /// Original value without any modifications.
    /// </summary>
    public T Base;
    
    /// <summary>
    /// This values changes as the modifiers being added or removed.
    /// </summary>
    /// <returns>Modified value</returns>
    public T Eval => _evaluate();
    
    /// <summary>
    /// Modified value should be calculated as:
    /// <br/>
    /// <i>Base + sum (immediates) + Base * sum (proportions)</i>
    /// <br/>
    /// Don't ask me why the proportions are added according to the base.
    /// </summary>
    /// <returns>Modified value</returns>
    protected abstract T _evaluate();
    
    protected readonly HashSet<ModifierImmediate> _modifiersImmediate = new();
    protected readonly HashSet<ModifierProportion> _modifiersProportion = new();
    
    protected ModifiableNumber(T @base) { Base = @base; }

    public void AddModifierImmediate(ModifierImmediate m)
    {
        _modifiersImmediate.Add(m);
    }

    public void AddModifierProportion(ModifierProportion m)
    {
        _modifiersProportion.Add(m);
    }
    
    public void RemoveModifierImmediate(ModifierImmediate m)
    {
        _modifiersImmediate.Remove(m);
    }
    
    public void RemoveModifierProportion(ModifierProportion m)
    {
        _modifiersProportion.Remove(m);
    }
}
