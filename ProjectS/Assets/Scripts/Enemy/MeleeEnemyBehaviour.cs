using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MeleeEnemyBehaviour : EnemyBehaviour
{
    [SerializeField] 
    private List<Ability> abilityList;
    private CoroutineRunner _coroutineRunner;
    private float _mana;
    private HealthSystem _healthSystem;
    private bool _isUsingAb=false;
    private float _attackCooldown =0;
    private float _abilityCooldown =0;
    private float _distance;
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDeath += Die;
        _healthSystem.OnTakeDamage+=TakeDamageAnim;
        _mana = _characteristics.secondCharDic["MaxMana"];
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
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
        _distance = Vector3.Distance(transform.position, _player.transform.position);
        if (_distance <= _detectionRadius||_isAggro)
        {
            _abilityCooldown += Time.deltaTime;
            _attackCooldown += Time.deltaTime;
            _isAggro = true;
            _isUsingAb = CheckForUsing();
            if (_distance<=abilityList[0].range)
            {
                Idle();
                agent.SetDestination(transform.position);
                Vector3 targetDirection = _player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 6);
                if ( _attackCooldown > 2f && !_isUsingAb)
                {
                    _attackCooldown = 0;
                    Attack();
                }
                
            }
            else if (_distance<=abilityList[1].range   && !_isUsingAb && _mana>=abilityList[1].manaCost && _abilityCooldown>5f)
            {
                Idle();
                agent.SetDestination(transform.position);
                Vector3 targetDirection = _player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation =
                    Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 6);
                _mana -= abilityList[1].manaCost;
                _attackCooldown = 0;
                _abilityCooldown = 0;
                UseAbility(1);
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
    public override void ChasePlayer()
    {
        agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        agent.SetDestination(_player.transform.position);
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
    public override void Idle()
    {
        StopAllCoroutines();
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
    public override void Attack()
    {
        abilityList[0].Activate(gameObject,_coroutineRunner,_animator);
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
    public void UseAbility(int index)
    {
        abilityList[index].Activate(gameObject,_coroutineRunner,_animator);
    }
}
