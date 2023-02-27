using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Pools;
using Common.Helpers;

public class MonofireBulletEmitter : MonoBehaviour
{
    public abstract class BulletEmitterComponent
    {
        private GameObject _object;
        public GameObject GameObject => _object;

        [Header("Emitter Data")]
        public SelectivePool bulletPool;
        public int bulletIndex;
        public int fireRate;
        public float fireSpeed;
        public int bulletCount;
        public Vector3 acceleration;
        private int _currentBulletIndex;
        public int CurrentBulletIndex => _currentBulletIndex;
        public void ResetBulletIndex() => _currentBulletIndex = 0;
        private int _frameCounter;
        public void Start(GameObject @object)
        {
            this._object = @object;
        }
        public void Update()
        {
            _frameCounter++;
            OnUpdate(_frameCounter);
        }
        public abstract void OnUpdate(int frameCounteer);
        public abstract void NextBullet(Bullet bullet);
        public void Fire(Vector2 center)
        {
            var bullet = bulletPool.GetObject(bulletIndex) as Bullet;
            bullet.Transform.position = center;
            bullet.acceleration = acceleration;
            NextBullet(bullet);
            _currentBulletIndex++;
        }
    }
    [System.Serializable]
    public class BulletEmitterComponentMonofire : BulletEmitterComponent
    {
        [Header("Monofire Fields")]
        public Transform target;
        public override void OnUpdate(int frameCounter)
        {
            if (frameCounter % fireRate == 0)
            {
                Fire(GameObject.transform.position);
            }
        }
        public override void NextBullet(Bullet bullet)
        {
            bullet.direction = MathHelper.DirectionTo(bullet.transform.position, target.position) * fireSpeed;
        }
    }
    
    public BulletEmitterComponentMonofire monofire;
    // Start is called before the first frame update
    void Start()
    {
        monofire.Start(gameObject);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        monofire.Update();
    }
}
