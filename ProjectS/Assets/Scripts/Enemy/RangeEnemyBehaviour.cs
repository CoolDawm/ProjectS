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
    }

    protected override void FixedUpdate()
    {
        if (_player == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            if (Vector3.Distance(transform.position, _player.transform.position) <=_characteristics.charDic["attackRange"])
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
            ChasePlayer();
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

        Destroy(projectile, _characteristics.charDic["projectileLife"]);
    }

    public override void ChasePlayer()
    {
        _agent.speed = _characteristics.charDic["movementSpeed"];
        _agent.SetDestination(_player.transform.position);
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
