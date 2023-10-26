
using UnityEngine;
using System;
public class Utilities
{
    public bool CalculateChance(float percent)
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        if (randomValue <= percent)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
