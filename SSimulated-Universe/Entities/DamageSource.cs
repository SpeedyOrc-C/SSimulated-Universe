using SSimulated_Universe.Environment;
using SSimulated_Universe.Events;

namespace SSimulated_Universe.Entities;

public interface DamageSource { }
public record DamageSourceEntity(Entity Source, DamageMethod Method) : DamageSource;
public record DamageSourceEffect(Effect Source) : DamageSource;

public enum DamageMethod { BasicAttack, Skill, FollowUp, Ultimate }
