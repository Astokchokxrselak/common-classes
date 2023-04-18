using System;
using System.Collections;
using UnityEngine;

using Common3D;
using Common.Extensions;
namespace Common.Pools
{
    public class PhysicsPoolObject3D : MonoBehaviour, IPoolObject
    {
        public Transform Transform { get => transform; }
        public GameObject GameObject { get => gameObject; }
        public BasePool Pool { get; set; }

        Lazy<MobileEntity3D> mobileEntity;
        public MobileEntity3D MobileEntity
        {
            get => mobileEntity.Value;
        }
        public void Destroy()
        {
            Pool.Return(this);
        }

        new public Rigidbody rigidbody;
        new public Collider collider;
        public void Awake()
        {
            rigidbody = this.GetOrAddComponent<Rigidbody>();
            collider = this.GetComponent<Collider>();
            mobileEntity = new Lazy<MobileEntity3D>(() => this.GetOrAddComponent<MobileEntity3D>());
        }
        public void SetParent(Transform parent)
        {
            rigidbody.isKinematic = false;
            transform.parent = parent;
        }
        public void RemoveParent()
        {
            rigidbody.isKinematic = true;
            transform.parent = null;
        }
    }
}