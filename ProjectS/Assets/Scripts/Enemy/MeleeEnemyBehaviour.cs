using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MeleeEnemyBehaviour : EnemyBehaviour
{
    [SerializeField]
    private float _damage = 10f;
    [SerializeField]
    private float _attackCooldown =0;
    private NavMeshAgent _agent;
    public AbilitesManager abilitiesManager;
    public UnityEvent meleeAbilityEvent;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        abilitiesManager = GameObject.FindGameObjectWithTag("AbilitiesManager").GetComponent<AbilitesManager>();
        meleeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.MeleeAbility(_attackRange, _damage,gameObject)));
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDeath += Die;
    }

    protected override void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            _isAggro = true;
            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                Idle();
                _agent.SetDestination(transform.position);

                _attackCooldown += Time.deltaTime;
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
    public override void ChasePlayer()
    {
        StartCoroutine(ChasePlayerCoroutine());
    }
    public override void Idle()
    {
        StopAllCoroutines();
    }
    private IEnumerator ChasePlayerCoroutine()
    {
        while (_isAggro)
        {
            _agent.SetDestination(_player.transform.position);
            yield return null;
        }
    }
    public override void Attack()
    {
        meleeAbilityEvent.Invoke();
        Debug.Log("Attack");
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
