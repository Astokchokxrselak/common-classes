using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Pools;
using Common.Helpers;

public class WeightedRandomPool : Pool
{
    public WeightedFloatRandom weights;
    public override void OnInitializePool()
    {
        if (count % objectReferences.Length != 0)
        {
            Debug.LogWarningFormat("WeightedRandomPool: Number of object instances is not divisible by number of object references({0}{1}, r={2})", weights.WeightCount, objectReferences.Length, weights.WeightCount % objectReferences.Length);
        }
        else if (weights.WeightCount != objectReferences.Length)
        {
            Debug.LogWarningFormat("WeightedRandomPool: Number of weights is not equal to number of object references");
        }
    }

    int numberAssigned = 0;
    int SubpoolObjectCount => count / objectReferences.Length;
    public GameObject CurrentReference => objectReferences[numberAssigned / SubpoolObjectCount];

    protected override bool CanTakeMultipleObjects => true;
    public override GameObject ChooseObject()
    {
        // W/ 150 count
        // 3 refs
        // 0 - 50: ref1
        // 51 - 100: ref2
        // 101 - 150: ref3
        var @object = CurrentReference;
        numberAssigned++;
        return @object;
    }
    public IPoolObject GetObject(int type, bool active = true) => GetObject(type, Vector3.zero, Quaternion.identity, active);
    public IPoolObject GetObject(int type, Vector3 position, bool active = true) => GetObject(type, position, Quaternion.identity, active);
    public IPoolObject GetObject(int type, Vector3 position, Quaternion rotation, bool active = true)
    {
        if (type > objectReferences.Length)
        {
            throw new System.ArgumentOutOfRangeException("Type out of range of possible object references");
        }
        for (int i = type * SubpoolObjectCount; i < (type + 1) * SubpoolObjectCount; i++)
        {
            if (!Objects[i].GameObject.activeInHierarchy)
            {
                OnGetObject(ref Objects[i]);
                if (active)
                {
                    Objects[i].GameObject.SetActive(true);
                }
                Objects[i].Transform.rotation = rotation;
                Objects[i].Transform.position = position;
                Objects[i].Transform.parent = null;
                return Objects[i];
            }
        }
        return OnFailGetObject();
    }
    public IPoolObject GetObject()
    {
        var type = weights.GetIndex(Random.value);
        return GetObject(type);
    }
}
