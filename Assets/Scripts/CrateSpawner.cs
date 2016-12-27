using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField]
    private Vector2 SpawnArea;

    private ObjectPooler[] allObjPools;

    private void Awake()
    {
        allObjPools = GetComponents<ObjectPooler>();

        List<float> test = new List<float>();
        test.Add(1f);
        SpawnObjects(new CrateProbability(test, 10));
    }

    /// <summary>
    /// Number of total active crates
    /// </summary>
    public int totalSpawnedObjects
    {
        get
        {
            int total = 0;
            for (int i = 0; i < allObjPools.Length; ++i)
            {
                total += allObjPools[i].numberOfActiveObjects;
            }
            return total;
        }
    }

    /// <summary>
    /// Begin spawning new objects at random locations within the spawn area.
    /// CrateProbability contains information about which crates should appear, and how many total crates there are
    /// </summary>
    public void SpawnObjects(CrateProbability cp)
    {
        for (int i = 0; i < allObjPools.Length; ++i)
        {
            for (int j = 0; j < cp.CrateTypeAmount[i]; ++j)
            {
                float rand_x = Random.Range(-SpawnArea.x, SpawnArea.x);
                float rand_y = Random.Range(-SpawnArea.y, SpawnArea.y);

                GameObject crate = allObjPools[i].RetrieveCopy();
                crate.transform.position = new Vector3(rand_x, transform.position.y, rand_y);
            }
        }
    }
}
