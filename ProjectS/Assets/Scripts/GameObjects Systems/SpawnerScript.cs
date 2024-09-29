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
    [SerializeField]
    private int _spawnStartingAmount;
    public bool spawningStart=false;
    private float _timer;
    private Utilities _utils = new Utilities();
    private EnemyGroup _enemyGroup;

    private void Start()
    {
        _timer = _timeSpawn;
        _enemyGroup = GetComponent<EnemyGroup>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player"&&spawningStart==false)
        {
            spawningStart = true;
        }
    }

    private void FixedUpdate()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            _timer = _timeSpawn;
            if (_childCount < _maxEnemy&& spawningStart==true)
            {
                _childCount++;
                _enemyGroup.enemGroup.Add(Instantiate(_enemyPrefab,_utils.GetRandomVector(gameObject.transform.position) , Quaternion.identity));
            }
        }
    }
    public EnemyGroup GetEnemyGroup()
    {
        return _enemyGroup;
    }
    public GameObject GetEnemyPrefab()
    {
        return _enemyPrefab;
    }
}

