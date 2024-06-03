namespace SSimulated_Universe.Universe;

public interface IDamageSource { }

public record DamageFromEntity(Entity Source, ActionType ActionType) : IDamageSource;

public record DamageFromEffect(Effect Source) : IDamageSource;

public enum ActionType { BasicAttack, Skill, FollowUp, Ultimate }
