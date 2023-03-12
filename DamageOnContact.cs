using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnContact : MonoBehaviour
{
    public float damage;
    public string targetTag;
    public bool damageOnceWhileAlive;
    private bool _dealtDamage;
    private void OnTriggerEnter(Collider collision)
    {
        if (_dealtDamage && damageOnceWhileAlive)
            return;
        if (string.IsNullOrWhiteSpace(targetTag) || collision.gameObject.CompareTag(targetTag))
        {
            print(collision.gameObject.tag);
            IDamageable target = collision.transform.parent.GetComponentInChildren<IDamageable>();
            if (target != null)
            {
                target.TakeDamage(damage, null);
                _dealtDamage = true;
            }
        }
    }
    private void OnEnable()
    {
        _dealtDamage = false;
    }
}
