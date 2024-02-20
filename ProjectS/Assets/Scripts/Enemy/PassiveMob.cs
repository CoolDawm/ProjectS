using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PassiveMob : EnemyBehaviour
{
    [SerializeField] private float _attackCooldown = 0;
    [SerializeField] private Ability _ability;
    private CoroutineRunner _coroutineRunner;

    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        _characteristics = gameObject.GetComponent<Characteristics>();
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDeath += Die;
        healthSystem.OnTakeDamage += TakeDamageAnim;
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        _attackRange = 2.5f;
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


        if (_isAggro)
        {
            if (Vector3.Distance(transform.position, _target.transform.position) <= _attackRange)
            {
                Idle();
                agent.SetDestination(transform.position);
                Vector3 targetDirection = _target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
                _attackCooldown += Time.deltaTime;
                if (_attackCooldown > 2f)
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

    public override void ChasePlayer()
    {
        agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        agent.SetDestination(_target.transform.position);
    }

    public override void Patrool()
    {
        changePositionTimer += Time.deltaTime;
        if (changePositionTimer >= 6f)
        {
            if (agent.remainingDistance <= agent.stoppingDistance) //done with path
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

    public override void Idle()
    {
        StopAllCoroutines();
    }

    public override void Attack()
    {
        //StartCoroutine(ActivateAbility())
        _animator.SetTrigger(_ability.animName);
        _ability.Activate(gameObject, _coroutineRunner);
    }

    public override void Die()
    {
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
        _isAggro = true;
        _animator.SetTrigger("Take Damage");
    }

    IEnumerator ActivateAbility()
    {
        float timer = 0;
        _animator.SetTrigger(_ability.animName);
        while (timer <= _ability.animTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        _ability.Activate(gameObject, _coroutineRunner);
    }
}
