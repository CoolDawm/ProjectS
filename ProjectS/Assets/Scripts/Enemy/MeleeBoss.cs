using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MeleeBoss : BossBehaviour
{
    private float _mana;
    private HealthSystem _healthSystem;
    private float _abilityCooldown =0;
    private float _attackCooldown = 0;
    private NavMeshAgent _agent;
    private bool _isUsingAb=false;
    private float _distance;

    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        _attackRange = 15f;
        _healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>();
        //_characteristics.charBuffBuffer["MovementSpeed"] = 10f;
        _healthSystem.OnDeath += Die;
        _healthSystem.OnTakeDamage+=TakeDamageAnim;
    }

    protected override void FixedUpdate()
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
        _distance = Vector3.Distance(transform.position, _player.transform.position);
        if (_distance <= _detectionRadius||isAggro)
        {

            if (_distance <= _attackRange)
            {
                Idle();
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                _abilityCooldown += Time.deltaTime;
                _isUsingAb = CheckForUsing();

                if (_attackCooldown >= 5f && !_isUsingAb)
                {
                    Vector3 targetDirection = _player.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 1.5f);
                    _attackCooldown = 0;
                    Attack();
                }

                if (_abilityCooldown >= 8f && !_isUsingAb && _mana >= abilityList[2].manaCost)
                {
                    Vector3 targetDirection = _player.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
                    _abilityCooldown = 0;
                    _attackCooldown = 0;
                    _mana -= abilityList[2].manaCost;
                    UseAbility(2);
                }
                else if (_abilityCooldown >= 8f && !_isUsingAb && _mana >= abilityList[1].manaCost)
                {
                    Vector3 targetDirection = _player.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 3);
                    _abilityCooldown = 0;
                    _attackCooldown = 0;
                    _mana -= abilityList[1].manaCost;
                    UseAbility(1);
                }

            }
            else
            {
                ChasePlayer();
            }
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
        _agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        _agent.SetDestination(_player.transform.position);
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
    public override void TakeDamageAnim()
    {
        isAggro = true;
        _animator.SetTrigger("Take Damage");
    }
    public override void UseAbility(int index)
    {
        Debug.Log("Using");
        abilityList[index].Activate(gameObject,_coroutineRunner,_animator);
    }
    
    public override void Attack()
    {
        abilityList[0].Activate(gameObject,_coroutineRunner,_animator);
    }
    public override void Die()
    {
        Destroy(gameObject);
    }
}
