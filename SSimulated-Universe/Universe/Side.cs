using SSimulated_Universe.Utility.Modifiable.Number;

namespace SSimulated_Universe.Universe;

public class Side
{
    public readonly ModifiableInt MaxSkillPoint = new(5);
    
    public IEnumerable<Entity> Entities => _entities;
    public int Count => _entities.Count;
    public Entity EntityAt(int index) => _entities.ElementAt(index);
    public bool Contains(Entity entity) => _entities.Contains(entity);

    public void Prepend(Entity entity)
    {
        _entities.Insert(0, entity);
        Battle.Broadcast(o => o.EntityJoined(entity));
    }

    public void Append(Entity entity)
    {
        _entities.Add(entity);
        Battle.Broadcast(o => o.EntityJoined(entity));
    }

    public bool Remove(Entity entity) => _entities.Remove(entity);

    public Entity? FindLeft(Entity entity)
    {
        var index = _entities.IndexOf(entity);
        return index is -1 or 0 ? null : _entities[index - 1];
    }

    public Entity? FindRight(Entity entity)
    {
        var index = _entities.IndexOf(entity);
        if (index == -1 || index == _entities.Count - 1) return null;
        return _entities[index + 1];
    }

    public Entity ChooseRandom()
    {
        if (_entities.Count == 0)
            throw new Exception("No entity to pick.");

        var index = (int) (Random.Shared.NextInt64() % _entities.Count);
        return _entities[index];
    }

    /// <summary>
    /// Consume a specified number of skill points. Do nothing when there are no enough skill points.
    /// </summary>
    /// <param name="count">Number of skill points to use.</param>
    /// <returns>Whether there are enough skill points to use.</returns>
    public bool UseSkillPoint(int count)
    {
        if (count > _skillPoint) return false;
        
        _skillPoint -= count;
        Battle.Broadcast(o => o.SkillPointChanged(this, count));
        return true;
    }

    public void ChangeSkillPoint(int delta)
    {
        if (delta == 0) return;
        
        var oldSkillPoint = _skillPoint;
        _skillPoint = Math.Clamp(_skillPoint + delta, 0, MaxSkillPoint.Eval);
        var deltaSkillPoint = _skillPoint - oldSkillPoint;

        if (deltaSkillPoint == 0) return;
        
        Battle.Broadcast(o => o.SkillPointChanged(this, deltaSkillPoint));
    }
    
    private readonly List<Entity> _entities = new();
    private int _skillPoint;
    
    private readonly Battle Battle;
    public Side(Battle battle) => Battle = battle;
}
