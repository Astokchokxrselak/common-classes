using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Pools;
using Common.Helpers;

public class Bullet : PoolObject
{
    public IDamageable creator;
    public Vector3 direction, directionAtSpawn;
    [HideInInspector]
    public Vector2 velocity;
    public Vector3 acceleration;
    public float damage;

    public BulletInfo bulletInfo;
    [System.Flags]
    public enum BulletInfo
    {
        None = 0,
        DontDieOffscreen = 1,
        Piercing = 2
    }
    public bool HasInfo(BulletInfo info) => (bulletInfo & info) != 0;
    public enum BulletState
    {

    }
    private void Update()
    {
        NormalAI();
    }
    private void FixedUpdate()
    {
        DefaultMotionAI();
        Accelerate();
        CollisionUpdate();
    }

    private const int _SharedOverlapArrayCount = 9;
    private static Collider2D[] _SharedOverlapArray = new Collider2D[_SharedOverlapArrayCount];
    [Header("Collision Information")]
    public string targetTag;
    public Vector2 pivot;
    public Vector2 size;
    void CollisionUpdate()
    {
        var hit = Physics2D.OverlapBoxNonAlloc((Vector2)transform.position + new Vector2(-size.x / 2f, size.y / 2f) + pivot, size, transform.eulerAngles.z, _SharedOverlapArray);
        for (int i = 0; i < hit; i++)
        {
            var target = _SharedOverlapArray[i];
            if (target && targetTag == null || target.CompareTag(targetTag))
            {
                var damageable = target.GetComponentInChildren<IDamageable>();
                if (damageable.Health > 0)
                {
                    damageable.TakeDamage(damage, creator);
                    if (HasInfo(BulletInfo.Piercing))
                    {
                        Destroy();
                    }
                }
            }
        }
    }
    private void OnEnable()
    {
        directionAtSpawn = direction;
    }
    void NormalAI()
    {
        velocity = direction;
    }
    void Accelerate()
    {
        //  direction += MathHelper.VectorRotatedByVector(acceleration * Time.fixedDeltaTime, directionAtSpawn);
    }
    void DefaultMotionAI()
    {
        transform.Translate(velocity * Time.fixedDeltaTime);
    }
    private void OnBecameInvisible()
    {
        if (!HasInfo(BulletInfo.DontDieOffscreen))
        {
            if (gameObject.scene.isLoaded)
            {
                Destroy();
            }
        }
    }
}
