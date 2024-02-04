
using UnityEngine;
using System;
using UnityEngine.AI;
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
    protected bool RandomPoint(Vector3 center, float range, out Vector3 result)// Redo it in Utils
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas)) //navmesh doc https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
