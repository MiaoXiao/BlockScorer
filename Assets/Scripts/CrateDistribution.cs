using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class CrateDistribution
{
    public List<int> CrateTypeDistribution= new List<int>();

    public int Total
    {
        get
        {
            int total = 0;
            foreach(int numb in CrateTypeDistribution)
            {
                total += numb;
            }
            return total;
        }
    }
}
