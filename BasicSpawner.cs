using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Common;
using Common.Helpers;
using Common.Pools;
public class BasicSpawner : MonoBehaviour
{
    const float SpawnDelay = 0.02f;

    public float spawnRate;
    public float spawnAccel;
    public Timer spawnTimer = SpawnDelay;

    public Pool pool;
    List<IPoolObject> spawnedEntities;
    private void Awake()
    {
        spawnedEntities = new();
    }
    void FixedUpdate()
    {
        // if (!DinoGameManager.Done)
        // {
            CheckSpawnedOffscreen();
            TrySpawnNext();
        // }
    }
    void TrySpawnNext()
    {
        if (spawnTimer.IncrementHit(true, false, true) && Random.value < spawnRate)
        {
            var human = pool.GetRandomObject();
            human.Transform.parent = null;
            human.Transform.position = (Vector2)Camera.main.transform.position + CameraHelper.WorldWidth() * Vector2.right;
            spawnedEntities.Add(human);
        }
    }
    void CheckSpawnedOffscreen()
    {
        for (int i = 0; i < spawnedEntities.Count; i++)
        {
            if (CameraHelper.IsOffscreenWorld(spawnedEntities[i].Transform.position).x == -1)
            {
                var entity = spawnedEntities[i].Transform.GetComponentInChildren<MobileEntity>();
                if (CameraHelper.IsOffscreenWorld(entity.Right).x == -1)
                {
                    spawnedEntities[i].Destroy();
                    spawnedEntities.RemoveAt(i);
                }
            }
        }
    }
}
