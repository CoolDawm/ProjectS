using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SimpleSummonsMeleeBehaviour : EnemyBehaviour
{
    [SerializeField] 
    private Ability _ability;
    private CoroutineRunner _coroutineRunner;
    private NavMeshAgent _agent;
    private float _attackCooldown=0;
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
        if (_player == null)
        {
            return;
        }

        if (_agent.hasPath)
        {
            _animator.SetBool("Move Forward Fast", true);
        }
        else
        {
            _animator.SetBool("Move Forward Fast", false);

        }
        _isAggro = true;
        if (Vector3.Distance(transform.position, _player.transform.position) <= _ability.range)
        {
            Idle();
            _agent.SetDestination(transform.position);
            _attackCooldown += Time.deltaTime;
            if (!_ability.abilityIsActive&&_attackCooldown>=2.5f)
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

    public override void ChasePlayer()
    {
        _agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        _agent.SetDestination(_player.transform.position);
    }
    public override void Idle()
    {
        StopAllCoroutines();
    }
    public override void Attack()
    {
        _ability.Activate(gameObject,_coroutineRunner,_animator);
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
    public override void TakeDamageAnim()
    {
        _animator.SetTrigger("Take Damage");
    }
}
