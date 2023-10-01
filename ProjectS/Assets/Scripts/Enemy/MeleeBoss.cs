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
    [SerializeField]
    private float _bombTimer = 0f;
    private bool _boom;
    private NavMeshAgent _agent;
    public AbilitesManager abilitiesManager;
    public UnityEvent meleeAbilityEvent;
    public UnityEvent shieldAbilityEvent;
    public UnityEvent meleeAoeAbilityEvent;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        abilitiesManager = GameObject.FindGameObjectWithTag("AbilitiesManager").GetComponent<AbilitesManager>();
        meleeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.MeleeAbility(_characteristics.charDic["meleeRange"], _damage, gameObject)));
        shieldAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.Shield(15, gameObject)));
        meleeAoeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.MeleeAoe(15, gameObject)));
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        healthSystem.OnDeath += Die;
    }

    protected override void FixedUpdate()
    {
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            _isAggro = true;
            if (Vector3.Distance(transform.position, _player.transform.position) <=_characteristics.charDic["meleeRange"])
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
    public override void Die()
    {
        Destroy(gameObject);
    }
}
