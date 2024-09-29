using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public List<Transform> spawnPoints=new List<Transform>();
    public GameObject[] spawnerPrefabs;
    private SceneLevelManager _stageStateManager;
    //public GameObject plane;

    void Start()
    {
        StartCoroutine(AddSpawnPointsCoroutine());
    }
    private IEnumerator AddSpawnPointsCoroutine()
    {
        yield return new WaitForSeconds(1);
        RoomInfo[] roomInfoList = FindObjectsOfType<RoomInfo>();
        foreach (RoomInfo roomInfo in roomInfoList)
        {
            spawnPoints.AddRange(roomInfo.spawnerSpwnPoints);
        }
        GenerateLevel();

        yield return null;
    }
    void GenerateLevel()
    {
        foreach (Transform spawnPoint in spawnPoints)
        {
            GameObject randomSpawner = spawnerPrefabs[Random.Range(0, spawnerPrefabs.Length)];
            Instantiate(randomSpawner, spawnPoint.position, Quaternion.identity);
        }
    }
}
