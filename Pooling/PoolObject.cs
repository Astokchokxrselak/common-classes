using System.Collections;
using UnityEngine;

namespace Common.Pools
{
    /// <summary>
    /// The base class for an object that belongs to a pool.
    /// </summary>
    public interface IPoolObject
    {
        public Transform Transform { get; }
        public GameObject GameObject { get; }
        public BasePool Pool { get; set; }
        public void Kill()
        {
            GameObject.SetActive(false);
        }
        public void Destroy()
        {
            Pool.Return(this);
        }
    }
    public class PoolObject : MonoBehaviour, IPoolObject
    {
        public Transform Transform { get => transform; }
        public GameObject GameObject { get => gameObject; }
        private BasePool pool;
        public BasePool Pool { get => pool; set { pool = value; print("pool changed"); } }
        public void Destroy()
        {
            try
            {
                Pool.Return(this);
            }
            catch (System.NullReferenceException)
            {
                print(gameObject.activeInHierarchy);
                UnityEditor.Selection.activeGameObject = gameObject;
                Debug.Break();
            }
        }
    }
}