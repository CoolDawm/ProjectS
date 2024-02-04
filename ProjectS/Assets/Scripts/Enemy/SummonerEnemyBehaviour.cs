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
    private Ability _ability;
    [SerializeField]
    private GameObject _summonPrefab;
    private CoroutineRunner _coroutineRunner;
    private NavMeshAgent _agent;
    private float _attackCooldown = 0f;
    protected override void Start()
    {
        base.Start();
        _attackRange = 13f;
        _detectionRadius = 15f;
        _agent = GetComponent<NavMeshAgent>();
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        healthSystem.OnTakeDamage+=TakeDamageAnim;
        healthSystem.OnDeath += Die;
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
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {
            _isAggro = true;
            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                Vector3 targetDirection = _player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
                if (_attackCooldown > 6f)
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
        Debug.Log("Summon");
        Vector3 summonPosition = transform.position + Random.insideUnitSphere *5; 
        Instantiate(_summonPrefab, summonPosition, Quaternion.identity); 
    }

    public override void ChasePlayer()
    {
        _agent.SetDestination(_player.transform.position);
        _agent.speed = _characteristics.secondCharDic["MovementSpeed"];
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

