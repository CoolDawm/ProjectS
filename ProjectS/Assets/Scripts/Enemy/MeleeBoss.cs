using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.AI;
using UnityEngine.Events;

public class MeleeBoss : BossBehaviour
{
    [SerializeField]
    private float _damage = 10f;
    [SerializeField]
    private float _attackCooldown = 0;
    
    private NavMeshAgent _agent;
    public AbilitesManager abilitiesManager;
    public UnityEvent meleeAbilityEvent;
    public UnityEvent shieldAbilityEvent;
    public UnityEvent suicideAbilityEvent;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        abilitiesManager = GameObject.FindGameObjectWithTag("AbilitiesManager").GetComponent<AbilitesManager>();
        meleeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.MeleeAbility(_attackRange, _damage, gameObject)));
        shieldAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.Shield(15, gameObject)));
        suicideAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.Suicide(gameObject)));
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
                _abilityCooldown += Time.deltaTime;
                if (_attackCooldown > 3f)
                {
                    _attackCooldown = 0;
                    Attack();
                }
                if (_abilityCooldown > 10f)
                {
                    _abilityCooldown = 0;
                    UseAbility();
                }
            }
            else
            {
                ChasePlayer();
                Debug.Log("gsg");
            }
        }
        else
        {
            _isAggro = false;
            ChasePlayer();
        }
    }
    public void Die()
    {
        suicideAbilityEvent.Invoke();
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
    public override void UseAbility()
    {
        shieldAbilityEvent.Invoke();
    }
    public override void Attack()
    {
        meleeAbilityEvent.Invoke();
        Debug.Log("Attack");
    }
}
