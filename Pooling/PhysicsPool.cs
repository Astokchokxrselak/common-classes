using System.Collections;
using UnityEngine;

namespace Common.Pools
{
    public class PhysicsPool : MultiObjectPool
    {
        public override IPoolObject OnInitializeObject(GameObject gameObj)
        {
            var obj = gameObj.AddComponent<PhysicsPoolObject>();
            obj.Pool = this;
            return obj;
        }
        public override void OnDestroyedObject(IPoolObject poolObject)
        {
            var bullet = poolObject as PhysicsPoolObject;
            bullet.RemoveParent();
        }
        public PhysicsPoolObject GetObject(Transform parent)
        {
            var obj = GetObject() as PhysicsPoolObject;
            obj.Transform.parent = parent;
            return obj;
        }
        public PhysicsPoolObject GetObject(Vector2 position, Transform parent)
        {
            var obj = GetObject(parent);
            obj.Transform.position = position;
            return obj;
        }
    }
}