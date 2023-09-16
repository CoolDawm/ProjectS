using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonsBehaviour : EnemyBehaviour
{
    [SerializeField]
    private float _damage = 15f;
    [SerializeField]
    private float _attackCooldown = 0;
    private NavMeshAgent _agent;


    protected override void Start()
    {
        base.Start();
        _attackRange = 2f;
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            _isAggro = true;
            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                Idle();
                _agent.SetDestination(transform.position);          
                Attack();
                Destroy(gameObject);
            }
            else
            {
                ChasePlayer();
            }
        }
        else
        {
            _isAggro = false;
            ChasePlayer();
        }
    }

    public override void ChasePlayer()
    {
        StartCoroutine(ChasePlayerCoroutine());
    }
    public override void Idle()
    {
        StopAllCoroutines();
    }

    private IEnumerator ChasePlayerCoroutine()
    {
        while (_isAggro)
        {
            _agent.SetDestination(_player.transform.position);
            yield return null;
        }
    }

    public override void Attack()
    {
        _player.GetComponent<HealthSystem>().TakeDamage(_damage);
        Debug.Log("Boom");
    }
}