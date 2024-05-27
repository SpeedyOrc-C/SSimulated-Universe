namespace SSimulated_Universe.Universe;

public interface IDamageSource { }

public record DamageFromEntity(Entity Source, DamageMethod Method) : IDamageSource;

public record DamageFromEffect(Effect Source) : IDamageSource;

public enum DamageMethod { BasicAttack, Skill, FollowUp, Ultimate }
