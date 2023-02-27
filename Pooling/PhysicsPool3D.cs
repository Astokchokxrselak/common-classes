using System.Collections;
using UnityEngine;

namespace Common.Pools
{
    public class PhysicsPool3D : MultiObjectPool
    {
        public override IPoolObject OnInitializeObject(GameObject gameObj)
        {
            var obj = gameObj.AddComponent<PhysicsPoolObject3D>();
            obj.Pool = this;
            return obj;
        }
        public override void OnDestroyedObject(IPoolObject poolObject)
        {
            var bullet = poolObject as PhysicsPoolObject3D;
            bullet.RemoveParent();
            bullet.rigidbody.velocity = Vector3.zero;
        }
        public PhysicsPoolObject3D GetObject(Transform parent)
        {
            var obj = GetObject() as PhysicsPoolObject3D;
            obj.Transform.parent = parent;
            return obj;
        }
        public PhysicsPoolObject3D GetObject(Vector3 position, Transform parent)
        {
            var obj = GetObject(parent);
            obj.Transform.position = position;
            return obj;
        }
    }
}