using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Pools;
public class Bullet3DPoolObject : MonoBehaviour, IPoolObject
{
    public Transform Transform => transform;
    public GameObject GameObject => gameObject;
    public BasePool Pool { get; set; }
    public Bullet3D Bullet { get; set; }
}
