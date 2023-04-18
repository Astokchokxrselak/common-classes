using System;
using System.Collections;
using UnityEngine;

using Common.Extensions;
namespace Common.Pools
{
    public class PhysicsPoolObject : MonoBehaviour, IPoolObject
    {
        public Transform Transform { get => transform; }
        public GameObject GameObject { get => gameObject; }
        public BasePool Pool { get; set; }

        Lazy<MobileEntity> mobileEntity;
        public MobileEntity MobileEntity
        {
            get => mobileEntity.Value;
        }
        public void Destroy()
        {
            Pool.Return(this);
        }

        new public Rigidbody2D rigidbody2D;
        new public Collider2D collider2D;
        public void Awake()
        {
            rigidbody2D = this.GetOrAddComponent<Rigidbody2D>();
            collider2D = this.GetComponent<Collider2D>();
            mobileEntity = new Lazy<MobileEntity>(() => this.GetOrAddComponent<MobileEntity>());
        }
        public void SetParent(Transform parent)
        {
            rigidbody2D.simulated = false;
            transform.parent = parent;
        }
        public void RemoveParent()
        {
            rigidbody2D.simulated = true;
            transform.parent = null;
        }
    }
}