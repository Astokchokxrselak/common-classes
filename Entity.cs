using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
public abstract class Entity : MobileEntity, IDamageable
{
    public float Health { get; set; }
    public float MaxHealth { get; set; }
    public virtual void TakeDamage(float damage, IDamageable other) { }
    public abstract void AI();
    private void Update()
    {
        AI();
    }
}
