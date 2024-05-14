using SSimulated_Universe.Entities;

namespace SSimulated_Universe.Universe;

public class Sides
{
    public readonly Side Left;
    public readonly Side Right;

    public Sides(Notifier notifier)
    {
        Left = new Side(notifier);
        Right = new Side(notifier);
    }
    
    public IEnumerable<Entity> Entities => Left.Entities.Concat(Right.Entities);

    public void LeftPrepend(Entity entity) => Left.Prepend(entity);
    public void RightPrepend(Entity entity) => Right.Prepend(entity);
    public void LeftAppend(Entity entity) => Left.Append(entity);
    public void RightAppend(Entity entity) => Right.Append(entity);

    public Side SideOf(Entity entity)
    {
        if (Left.Contains(entity)) return Left;
        if (Right.Contains(entity)) return Right;
        throw new Exception("Cannot find the entity.");
    }

    public Side OppositeSideOf(Entity entity)
    {
        if (Left.Contains(entity)) return Right;
        if (Right.Contains(entity)) return Left;
        throw new Exception("Cannot find the entity.");
    }
}
