using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonsBehaviour : EnemyBehaviour
{
    [SerializeField] 
    private Ability _ability;
    private CoroutineRunner _coroutineRunner;
    private NavMeshAgent _agent;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        healthSystem.OnDeath += Die;
        healthSystem.OnTakeDamage+=TakeDamageAnim;
        _ability.abilityIsActive = false;
    }

    protected override void Update()
    {
        if (_target == null)
        {
            ChangeTarget();
            if (_target == null)
            {
                return;
            }
            
        }
        if (agent.hasPath)
        {
            _animator.SetBool("Walk Forward", true);
        }
        else
        {
            _animator.SetBool("Walk Forward", false);

        }
        Agroed();
        if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
        {
            Idle();
            _agent.SetDestination(transform.position);
            if (!_ability.abilityIsActive)
            {
                Debug.Log("Boom");
                Attack();
            }
           
        }
        else
        {
            ChasePlayer();
        }


    }

    public override void ChasePlayer()
    {
        _agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        _agent.SetDestination(_target.transform.position);
    }
    public override void Idle()
    {
        StopAllCoroutines();
    }
    public override void Attack()
    {
        _ability.Activate(gameObject,_coroutineRunner);
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
        base.Die();
        Destroy(gameObject);
    }
    public override void Agroed()
    {
        _isAggro = true; 
        ChangeTarget();
    }

    public override void ChangeTarget()
    {
        if (GameObject.FindGameObjectWithTag("Summon") != null)
        {
            _target = GameObject.FindGameObjectWithTag("Summon");
        }
        else
        {
            _target = GameObject.FindGameObjectWithTag("Player");
        }
    }
    public override void TakeDamageAnim()
    {
        _animator.SetTrigger("Take Damage");
    }
}