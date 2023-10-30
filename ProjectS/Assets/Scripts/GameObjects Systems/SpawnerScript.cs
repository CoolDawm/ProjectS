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
    private float _timeSpawn = 2f;
    [SerializeField]
    private int _childCount = 0;
    private float _timer;
    private Utilities _utils = new Utilities();

    private void Start()
    {
        _timer = _timeSpawn;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = _timeSpawn;
            if (_childCount < _maxEnemy)
            {
                _childCount++;
                Instantiate(_enemyPrefab,_utils.GetRandomVector(gameObject.transform.position) , Quaternion.identity);
            }
        }
    }
}

