using UnityEngine;

public interface IDamageable
{
    public float Health { get; set; }
    public float MaxHealth { get; }
    public abstract void TakeDamage(float damage, IDamageable other);
    public virtual bool OnTakeDamage(float damage, IFighter other) => true; // return false to negate damage
    public virtual bool OnDead(IDamageable killer) => true; // return false to revive
}

public interface IFighter
{
    public virtual bool OnDealDamage(float damage, IDamageable other)
    {
        Debug.Log(this);
        return true; // return false to deal no damage
    }
    public virtual bool OnKill(IDamageable victim) => true; // return true to revive victim
}