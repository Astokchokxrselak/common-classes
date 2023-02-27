using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

using Common.Extensions;
using UnityEditor;

namespace Common.Pools
{
    /// <summary>
    /// The base class for all Pool Objects. Can be used for projectiles, enemies, etc.
    /// Anything that needs to be reloaded or respawned.
    /// </summary>
    /// <returns></returns>
    public abstract class BasePool : MonoBehaviour
    {
        public string poolName;
        [HideInInspector]
        public Transform pool;
        public GameObject[] objectReferences; // by default selects first object
        public IPoolObject[] Objects;
        public int count = 1;
        // Start is called before the first frame update

        protected virtual bool CanTakeMultipleObjects => false;
        void ValidatePool()
        {
            if (objectReferences.Length > 1 && !CanTakeMultipleObjects)
            {
                Debug.LogWarning("In a default Pool object, only the first element of objectReferences will ever be instantiated. Consider extending the Pool class or inserting exactly one element into the pool.");
            }
            if (objectReferences.Length == 0)
            {
                throw new System.Exception("The " + GetType() + " in " + name + " is empty. Did you forget to add reference objects?");
            }
        }
        public virtual void OnInitializePool()
        {

        }
        public virtual void OnInitializeObjectsArray()
        {

        }
        /// <summary>
        /// This method determines the reference added to the list of spawned objects in the pool.
        /// </summary>
        /// <returns></returns>
        public virtual GameObject ChooseObject()
        {
            return objectReferences[0];
        }
        public virtual IPoolObject OnInitializeObject(GameObject obj)
        {
            var poolObject = obj.GetOrAddComponent<PoolObject>();
            return poolObject;
        }
        public virtual void OnDestroyedObject(IPoolObject poolObject)
        {

        }
        public virtual void OnGetObject(ref IPoolObject obj)
        {

        }
        public virtual IPoolObject OnFailGetObject()
        {
            return null;
        }
        public virtual void OnGetObject(ref int i)
        {

        }
        public virtual void PostInitializeArray()
        {

        }
        #region Private initialization functions 
        void Awake()
        {
            ValidatePool();
            InitializePool();
            InitializeObjectsArray();
            InitializeObjects();
            PostInitializeArray();
        }
        void InitializePool()
        {
            pool = new GameObject(poolName).transform;
            OnInitializePool();
        }
        void InitializeObjectsArray()
        {
            Objects = new IPoolObject[count];
            OnInitializeObjectsArray();
        }
        void InitializeObjects()
        {
            for (int i = 0; i < Objects.Length; i++)
            {
                var obj = ChooseObject();
                var gameObj = Instantiate(obj, pool);
                gameObj.name = gameObj.name.Remove(gameObj.name.Length - 7);
                gameObj.SetActive(false);
                Objects[i] = OnInitializeObject(gameObj);
                Objects[i].Pool = this;
            }
        }
        #endregion
        #region GetObject() overloads
        /// <summary>
        /// Not recommended for performance sensitive pooling contexts. Consider extending BasePool.
        /// </summary>
        /// <returns></returns>
        public IPoolObject GetRandomObject(bool active = true)
        {
            var objs = Objects.Where((x) => !x.GameObject.activeInHierarchy);
            if (objs.Count() == 0) 
            { 
                return OnFailGetObject(); 
            }
            var obj = objs.Choice();
            if (active)
            {
                obj.GameObject.SetActive(true);
            }
            OnGetObject(ref obj);
            return obj;
        }
        public IPoolObject GetObject(bool active = true)
        {
            for (int i = 0; i < count; i++)
            {
                OnGetObject(ref i);
                if (!Objects[i].GameObject.activeInHierarchy)
                {
                    OnGetObject(ref Objects[i]);
                    if (active)
                    {
                        Objects[i].GameObject.SetActive(true);
                    }
                    Objects[i].Transform.parent = null;
                    return Objects[i];
                }
            }
            return OnFailGetObject();
        }
        public IPoolObject GetObject(Vector3 position)
        {
            return GetObject(position, Quaternion.identity);
        }
        public IPoolObject GetObject(Vector3 position, Vector2 scale)
        {
            return GetObject(position, Quaternion.identity, scale);
        }
        public IPoolObject GetObject(Vector3 position, Quaternion rotation)
        {
            var obj = GetObject();
            if (obj == null) return null;
            obj.Transform.position = position;
            obj.Transform.rotation = rotation;
            return obj;
        }
        public IPoolObject GetObject(Vector3 position, Quaternion rotation, Vector2 scale)
        {
            var obj = GetObject();
            if (obj == null) return null;
            var gameObj = obj.GameObject;
            if (gameObj != null)
            {
                gameObj.transform.position = position;
                gameObj.transform.rotation = rotation;
                gameObj.transform.localScale = scale;
            }
            return obj;
        }
        #endregion
        #region Methods related to returning PoolObjects
        public void Return(IPoolObject poolObject)
        {
            if (!Objects.Contains(poolObject))
            {
                throw new System.ArgumentException("Cannot return a PoolObject to a pool it does not belong to!");
            }
            OnDestroyedObject(poolObject);
            poolObject.Transform.SetParent(pool.transform);
            poolObject.Kill();
        }
        #endregion
    }
}