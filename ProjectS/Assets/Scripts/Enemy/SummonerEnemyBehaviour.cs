using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class SummonerEnemyBehaviour : EnemyBehaviour
{
    
    [SerializeField]
    private float _rotationSpeed = 6f;
    [SerializeField] 
    private List<Ability> _abilityList;
    [SerializeField]
    private GameObject _summonPrefab;
    private CoroutineRunner _coroutineRunner;
    private NavMeshAgent _agent;
    private float _attackCooldown = 0f;
    private float _abilityCooldown =0;
    private float _distance;
    private float _mana;
    private bool _isUsingAb=false;
    private HealthSystem _healthSystem;
    protected override void Start()
    {
        base.Start();
        _attackRange = 13f;
        _detectionRadius = 15f;
        _agent = GetComponent<NavMeshAgent>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnTakeDamage+=TakeDamageAnim;
        _healthSystem.OnDeath += Die;
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
        if (_distance <= _detectionRadius)
        {
            Agroed();
            if (_distance <= _attackRange)
            {
                
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                _abilityCooldown+= Time.deltaTime;
                Vector3 targetDirection = _target.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
                _isUsingAb = CheckForUsing();
                if (_abilityCooldown > 6f&& _abilityList[1].manaCost<=_mana&& !_isUsingAb)
                {
                    _mana -= _abilityList[1].manaCost;
                    _attackCooldown = 0;
                    _abilityCooldown = 0;
                    UseAbility(1);

                    //Attack();
                }
                else if (_attackCooldown >= 2 && !_isUsingAb)
                {
                    _attackCooldown = 0;
                    UseAbility(0);
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
        Vector3 summonPosition = transform.position + Random.insideUnitSphere *5; 
        Instantiate(_summonPrefab, summonPosition, Quaternion.identity); 
    }

    public override void ChasePlayer()
    {
        _agent.SetDestination(_target.transform.position);
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
        base.Die();
        Destroy(gameObject);
    }
    public override void TakeDamageAnim()
    {
        _isAggro = true;
        _animator.SetTrigger("Take Damage");
    }
    private bool CheckForUsing()
    {
        foreach (Ability ab in _abilityList)
        {
            if (ab.abilityIsActive)
            {
                return true;
            }
        }

        return false;
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
    public void UseAbility(int index)
    {
        _abilityList[index].Activate(gameObject,_coroutineRunner,_animator);
    }
}

