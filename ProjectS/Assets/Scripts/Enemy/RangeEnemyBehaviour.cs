using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangeEnemyBehaviour : EnemyBehaviour
{
    [SerializeField]
    private Transform shootingPosition;
    [SerializeField]
    private float _rotationSpeed = 6f;
    [SerializeField] 
    private List<Ability> abilityList;
    private NavMeshAgent _agent;
    public GameObject projectilePrefab;
    private float _attackCooldown = 0f;
    private float _abilityCooldown =0;
    private float _distance;
    public float projectileSpeed = 10f;
    private float _mana;
    private bool _isUsingAb=false;
    private CoroutineRunner _coroutineRunner;
    private HealthSystem _healthSystem;

    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        _healthSystem.OnDeath += Die;
        _healthSystem.OnTakeDamage+=TakeDamageAnim;
        _attackRange = detectionRadius - 3;
        _mana = _characteristics.secondCharDic["MaxMana"];
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();

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
        _distance = Vector3.Distance(transform.position, _target.transform.position);

        if (_distance <= _detectionRadius||_isAggro)
        {
            Agroed();
            if (_distance <=_attackRange)
            {
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                _abilityCooldown += Time.deltaTime;
                Vector3 targetDirection = _target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
                _isUsingAb = CheckForUsing();
                if (_attackCooldown > 1.5f && !_isUsingAb)
                {
                    _attackCooldown = 0;
                    Attack();
                }
                else if (_abilityCooldown>=3&&!_isUsingAb&&abilityList[0].manaCost<=_mana)
                {
                    _attackCooldown = 0;
                    _abilityCooldown = 0;
                    _mana -= abilityList[0].manaCost;
                    UseAbility(1);
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
        if (_mana > _characteristics.secondCharDic["MaxMana"])
        {
            _mana = _characteristics.secondCharDic["MaxMana"];
        }
        else
        {
            _mana += Time.deltaTime * _characteristics.secondCharDic["ManaRegen"]*25;
        }
        _healthSystem.healthBar.UpdateManaBar(_characteristics.secondCharDic["MaxMana"],_mana);
        _isUsingAb = false;
    }

    public override void Idle()
    {
        StopAllCoroutines();
    }

    public override void Attack()
    {
        UseAbility(0);
    }
    public void UseAbility(int index)
    {
        abilityList[index].Activate(gameObject,_coroutineRunner,_animator);
    }
    public override void ChasePlayer()
    {
        _agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        _agent.SetDestination(_target.transform.position);
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
    private bool CheckForUsing()
    {
        foreach (Ability ab in abilityList)
        {
            if (ab.abilityIsActive)
            {
                return true;
            }
        }

        return false;
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
        _isAggro = true;
        _animator.SetTrigger("Take Damage");
    }
}
