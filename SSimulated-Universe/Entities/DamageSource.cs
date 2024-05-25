using SSimulated_Universe.Environment;

namespace SSimulated_Universe.Entities;

public interface DamageSource { }
public record DamageSourceEntity(Entity Source, DamageMethod Method) : DamageSource;
public record DamageSourceEffect(EffectTimed Source) : DamageSource;

public enum DamageMethod { BasicAttack, Skill, FollowUp, Ultimate }
