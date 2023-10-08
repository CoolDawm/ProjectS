using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class SummonerEnemyBehaviour : EnemyBehaviour
{
    
    [SerializeField]
    private float _rotationSpeed = 3f;
    [SerializeField]
    private float _summonRadius = 6f;
    private NavMeshAgent _agent;
    public GameObject summonPrefab;
    public GameObject projectilePrefab;
    private float _attackCooldown = 0f;
    protected override void Start()
    {
        base.Start();
        _attackRange = 10f;
        _detectionRadius = 15f;
        _agent = GetComponent<NavMeshAgent>();
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        healthSystem.OnDeath += Die;
    }

    protected override void FixedUpdate()
    {


        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                StopAllCoroutines();
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                Vector3 targetDirection = _player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
                if (_attackCooldown > 8f)
                {
                    _attackCooldown = 0;
                    Attack();
                }
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
    public override void Idle()
    {
        StopAllCoroutines();
    }

    public override void Attack()
    {
        Debug.Log("Attack");
        Vector3 summonPosition = transform.position + Random.insideUnitSphere * _summonRadius; 
        GameObject newBeast = Instantiate(summonPrefab, summonPosition, Quaternion.identity); 
    }

    public override void ChasePlayer()
    {
        _agent.SetDestination(_player.transform.position);
        _agent.speed = _characteristics.charDic["movementSpeed"];
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}

