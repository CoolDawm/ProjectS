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
    public float projectileLifetime = 3f;
    
    protected override void Start()
    {
        base.Start();
        _attackRange = 10f;
        _detectionRadius = 15f;
        _agent = GetComponent<NavMeshAgent>();
    }

    protected override void FixedUpdate()
    {

        
            if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
            {

                if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
                {
                    _agent.SetDestination(transform.position);
                    _attackCooldown += Time.deltaTime;
                    Vector3 targetDirection = _player.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
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

        GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = transform.forward * projectileSpeed;

        Destroy(projectile, projectileLifetime);
    }

    public override void ChasePlayer()
    {
        _agent.SetDestination(_player.transform.position);
    }
}
