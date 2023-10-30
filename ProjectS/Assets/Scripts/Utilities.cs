
using UnityEngine;
using System;
using Random = UnityEngine.Random;

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
    public Vector3 GetRandomVector(Vector3 inputVector)
    {
        float randomX = Random.Range(-10f, 10f);
        float randomZ = Random.Range(-10f, 10f);
        Vector3 randomVector = new Vector3(randomX, 0f, randomZ);
        Vector3 resultVector = inputVector + randomVector;
        return resultVector;
    }
}
