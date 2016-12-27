using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateProbability
{
    public CrateProbability(List<float> probability, int total)
    {
        for (int i = 0; i < probability.Count; ++i)
        {
            CrateTypeAmount.Add(Mathf.FloorToInt(probability[i] * total));
        }

        Total = total;
    }

    public List<int> CrateTypeAmount = new List<int>();

    public int Total { get; }
}
