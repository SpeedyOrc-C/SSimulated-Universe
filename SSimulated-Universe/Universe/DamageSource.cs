namespace SSimulated_Universe.Universe;

public interface IDamageSource { }

public record DamageFromEntity(Entity Source) : IDamageSource;

public record DamageFromEffect(Effect Source) : IDamageSource;