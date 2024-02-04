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
    [SerializeField] 
    private Ability _ability;
    private CoroutineRunner _coroutineRunner;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        healthSystem.OnDeath += Die;
        healthSystem.OnTakeDamage+=TakeDamageAnim;
        _attackRange = detectionRadius - 3;
    }

    protected override void Update()
    {
        if (_player == null)
        {
            return;
        }
        if (agent.hasPath)
        {
            _animator.SetBool("Walk Forward", true);
        }
        else
        {
            _animator.SetBool("Walk Forward", false);

        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius||_isAggro)
        {
            _isAggro = true;
            if (Vector3.Distance(transform.position, _player.transform.position) <=_attackRange)
            {
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                Vector3 targetDirection = _player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
                if (_attackCooldown > 1.5f)
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
            Patrool();
        }
    }

    public override void Idle()
    {
        StopAllCoroutines();
    }

    public override void Attack()
    {
        _animator.SetTrigger("Projectile Attack");
        GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = transform.forward * projectileSpeed;
        projectile.GetComponent<ProjectileScript>().aim = "Player";
        projectile.GetComponent<ProjectileScript>().range = 15;
    }

    public override void ChasePlayer()
    {
        _agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        _agent.SetDestination(_player.transform.position);
    }
    public override void Patrool()
    {
        changePositionTimer += Time.deltaTime;
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
    public override void TakeDamageAnim()
    {
        _isAggro = true;
        _animator.SetTrigger("Take Damage");
    }
}
