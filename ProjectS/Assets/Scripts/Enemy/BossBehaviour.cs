using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBehaviour : MonoBehaviour, IEnemy, IEnemyMovement,IEnemyAbilitiesUse
{
    [SerializeField]
    protected List<Ability> abilityList;
    [SerializeField]
    protected float _detectionRadius = 5f;
    [SerializeField]
    private float _aggroRadius = 10f;
    [SerializeField]
    protected float _abilityCooldown = 0f;
    protected GameObject _player;
    protected Characteristics _characteristics; 
    protected Animator _animator;
    protected float _attackRange = 8f;
    protected CoroutineRunner _coroutineRunner;
    protected bool isAggro = false;
    // Implementing properties from IEnemy
    public float detectionRadius { get { return _detectionRadius; } }
    public float aggroRadius { get { return _aggroRadius; } }
    public float abilityCooldown { get { return _abilityCooldown; } }
    // Implementing properties from IEnemyMovement
    

    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        _animator = GetComponentInChildren<Animator>();

    }

    protected virtual void FixedUpdate()
    {
      
    }
    public abstract void Die();
    public abstract void UseAbility(int index);
    public abstract void Attack();
    public abstract void Idle();
    public abstract void ChasePlayer();
    public abstract void TakeDamageAnim();
}
