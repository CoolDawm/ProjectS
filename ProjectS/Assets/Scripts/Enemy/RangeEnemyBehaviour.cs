using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyBehaviour : EnemyBehaviour
{
    [SerializeField]
    private Transform shootingPosition;
    [SerializeField]
    private float _rotationSpeed = 3f;
    private NavMeshAgent _agent;
    public GameObject projectilePrefab;
    private float _attackCooldown = 0f;
    public float projectileSpeed = 10f;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        healthSystem.OnDeath += Die;
        _attackRange = detectionRadius - 3;
    }

    protected override void FixedUpdate()
    {
        if (_player == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            if (Vector3.Distance(transform.position, _player.transform.position) <=_attackRange)
            {
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                Vector3 targetDirection = _player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
                if (_attackCooldown > 3f)
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
            Patrool();
        }
    }

    public override void Idle()
    {
        StopAllCoroutines();
    }

    public override void Attack()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = transform.forward * projectileSpeed;

        Destroy(projectile, 4);
    }

    public override void ChasePlayer()
    {
        _agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        _agent.SetDestination(_player.transform.position);
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
