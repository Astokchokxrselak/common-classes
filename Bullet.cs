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

    public BulletInfo bulletInfo;
    [System.Flags]
    public enum BulletInfo
    {
        None = 0,
        DontDieOffscreen = 1
    }
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
        if ((bulletInfo & BulletInfo.DontDieOffscreen) == 0)
        {
            if (gameObject.scene.isLoaded)
            {
                Destroy();
            }
        }
    }
}
