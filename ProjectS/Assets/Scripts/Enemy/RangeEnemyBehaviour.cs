using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class RangeEnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    public float _detectionRadius = 15f; // –адиус обнаружени€ игрока
    [SerializeField]
    public float _aggroRadius = 15f; // Agro radius
    [SerializeField]
    private float _attackRange = 10f;
    private GameObject _player; // Player link
    private NavMeshAgent _agent; // NavMesh component
    private bool _isAggro = false; 
    private float _attackCooldown=0f;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // ѕровер€ем, находитс€ ли игрок в зоне обнаружени€ врага
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {
            // ≈сли игрок находитс€ в зоне агро и враг не атакует, начинаем преследование
            _isAggro = true;

            

            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                StopAllCoroutines();
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                if (_attackCooldown > 3f)
                {
                    _attackCooldown = 0;
                    Attack();
                }
            }
            else
            {
                StartCoroutine(ChasePlayer());
            }
        }
        else
        {
            _isAggro = false;
            StopCoroutine(ChasePlayer());
        }
    }
    public void Attack()
    {
        Debug.Log("Attack");
    }
    IEnumerator ChasePlayer()
    {
        while (_isAggro)
        {
            _agent.SetDestination(_player.transform.position);
            yield return null;
        }
    }
}
