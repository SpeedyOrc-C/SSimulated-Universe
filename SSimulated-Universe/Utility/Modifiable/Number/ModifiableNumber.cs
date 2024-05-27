using System.Numerics;

namespace SSimulated_Universe.Utility.Modifiable.Number;

public abstract class ModifiableNumber<T, TModifierImmediate, TModifierProportion>
    where T : INumber<T>
{
    /// <summary>
    /// Original value without any modifications.
    /// </summary>
    public readonly T Base;

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

    protected readonly HashSet<TModifierImmediate> ModifiersImmediate = new();
    protected readonly HashSet<TModifierProportion> ModifiersProportion = new();

    protected ModifiableNumber(T @base) { Base = @base; }

    public void AddModifierImmediate(TModifierImmediate m)
    {
        ModifiersImmediate.Add(m);
    }

    public void AddModifierProportion(TModifierProportion m)
    {
        ModifiersProportion.Add(m);
    }

    public void RemoveModifierImmediate(TModifierImmediate m)
    {
        ModifiersImmediate.Remove(m);
    }

    public void RemoveModifierProportion(TModifierProportion m)
    {
        ModifiersProportion.Remove(m);
    }
}
