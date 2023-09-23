using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnerScript : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private int _maxEnemy = 5;
    [SerializeField]
    private float timeSpawn = 2f;
    [SerializeField]
    private int _childCount = 0;
    [SerializeField]
    public float distance = 4;
    private float timer;


    private void Start()
    {
        timer = timeSpawn;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = timeSpawn;
            if (_childCount < _maxEnemy)
            {
                _childCount++;
                Instantiate(_enemyPrefab, Random.insideUnitCircle * distance, Quaternion.identity);
            }
        }
    }
}

