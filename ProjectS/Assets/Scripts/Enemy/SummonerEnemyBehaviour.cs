using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class SummonerEnemyBehaviour : EnemyBehaviour
{
    
    [SerializeField]
    private float _rotationSpeed = 3f;
    private NavMeshAgent _agent;
    public GameObject summonPrefab;
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
        if (_player == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            if (Vector3.Distance(transform.position, _player.transform.position) <= _characteristics.charDic["attackRange"])
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
        Vector3 summonPosition = transform.position + Random.insideUnitSphere *_characteristics.charDic["summonRadius"]; 
        GameObject newBeast = Instantiate(summonPrefab, summonPosition, Quaternion.identity); 
    }

    public override void ChasePlayer()
    {
        _agent.SetDestination(_player.transform.position);
        _agent.speed = _characteristics.charDic["movementSpeed"];
    }
    public override void Patrool()
    {
        changePositionTimer += Time.deltaTime;
        Debug.Log(changePositionTimer);
        if (changePositionTimer >= 6f)
        {
            if(agent.remainingDistance <= agent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(transform.position, range, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.red, 5f); //gizmos
                    agent.SetDestination(point);
                    changePositionTimer = 0f;
                }
            }
        }
        
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}

