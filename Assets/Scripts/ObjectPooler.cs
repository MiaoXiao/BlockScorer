﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    /// <summary>
    /// What object are we pooling?
    /// </summary>
    public GameObject ObjToPool;

    /// <summary>
    /// How many copies should we start with?
    /// </summary>
    [SerializeField]
    private int NumberOfCopies;

    /// <summary>
    /// Where to store the pooled objects?
    /// </summary>
    [SerializeField]
    private Transform StorageArea;

    /// <summary>
    /// Number of these objects active
    /// </summary>
    public int NumberOfActiveObjects
    {
        get
        {
            int count = 0;
            foreach (GameObject obj in PooledObjects)
            {
                if (obj.activeInHierarchy)
                {
                    count++;
                }
            }
            return count;
        }
    }

    private List<GameObject> PooledObjects = new List<GameObject>();

	// Use this for initialization
	void Start ()
    {
        for (int i = 0; i < NumberOfCopies; ++i)
        {
            GameObject new_obj = AddObjToPool();
            new_obj.SetActive(false);
        }
	}

    /// <summary>
    /// Retrieve
    /// </summary>
    public GameObject RetrieveCopy()
    {
        foreach(GameObject obj in PooledObjects)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                RemoveMomentum(obj);
                return obj;
            }
        }

        //All objects are currently in use, so create another.
        GameObject new_obj = AddObjToPool();
        new_obj.SetActive(true);
        RemoveMomentum(new_obj);
        return new_obj;
    }

    /// <summary>
    /// Create a new obj, add to storage area and to the list. Return the new obk
    /// </summary>
    private GameObject AddObjToPool()
    {
        GameObject copy = Instantiate(ObjToPool);
        copy.transform.SetParent(StorageArea);
        PooledObjects.Add(copy);
        return copy;
    }
    
    /// <summary>
    /// Remove all momentun from an object
    /// </summary>
    private void RemoveMomentum(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
