using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class SceneLevelManager : MonoBehaviour
{
    [SerializeField]
    private List<SpawnerScript> _spawnersList;
    [SerializeField]
    private int _spawnStartingAmount=0;
    private bool _isSpawned=false;
    private Utilities _utils = new Utilities();

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && _isSpawned == false)
        {
            Debug.Log("Player In Vicinity");
            Debug.Log(_spawnersList.Count);

            foreach (SpawnerScript spawner in _spawnersList)
            {
                GameObject enemyPrefab = spawner.GetEnemyPrefab();
                for (int i = 0; i < _spawnStartingAmount; i++)
                {
                    EnemyGroup enemyGroup = spawner.GetEnemyGroup();
                    enemyGroup.enemGroup.Add(Instantiate(enemyPrefab, _utils.GetRandomVector(spawner.transform.position), Quaternion.identity));
                }
            }
           
            _isSpawned = true;
        }else if (other.GetComponent<SpawnerScript>()&&other.isTrigger)
        {
            _spawnersList.Add(other.GetComponent<SpawnerScript>());
        }
    }
}
