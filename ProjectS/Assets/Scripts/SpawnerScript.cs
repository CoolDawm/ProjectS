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
                if (transform.childCount < _maxEnemy)
                {
                    Instantiate(_enemyPrefab, Random.insideUnitCircle * distance, Quaternion.identity, transform);
                }
            }
        }
    }

