using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CrateDistribution
{
    public List<float> CrateTypeDistribution= new List<float>();

    public int Total = 0;

    /// <summary>
    /// Returns the number of crates for a given type.
    /// </summary>
    public int RetrieveCrateAmount(int id)
    {
        return Mathf.FloorToInt(CrateTypeDistribution[id] * Total);
    }
}
