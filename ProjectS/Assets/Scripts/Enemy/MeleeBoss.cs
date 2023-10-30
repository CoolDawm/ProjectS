using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class MeleeBoss : BossBehaviour
{
    [SerializeField]
    private float _attackCooldown = 0;
    public AbilitesManager abilitiesManager;
    public UnityEvent meleeAbilityEvent;
    public UnityEvent shieldAbilityEvent;
    public UnityEvent SpeedScreamAbilityEvent;
    private NavMeshAgent _agent;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
        abilitiesManager = GameObject.FindGameObjectWithTag("AbilitiesManager").GetComponent<AbilitesManager>();
        meleeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.MeleeAbility(_characteristics.charDic["meleeRange"], _characteristics.charDic["damage"], gameObject,80)));
        shieldAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.Shield( gameObject,80)));
        SpeedScreamAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.SpeedScream(gameObject,"Enemy",70)));
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        healthSystem.OnDeath += Die;
    }

    protected override void FixedUpdate()
    {
        if (_player == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            _isAggro = true;
            _abilityCooldown += Time.deltaTime;
            if (_abilityCooldown > 20f)
            {
                _abilityCooldown = 0;
                UseAbility();
            }
            if (Vector3.Distance(transform.position, _player.transform.position) <=_characteristics.charDic["attackRange"])
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
    public override void UseAbility()
    {
        Debug.Log("Aility");
        shieldAbilityEvent.Invoke();
        SpeedScreamAbilityEvent.Invoke();
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
