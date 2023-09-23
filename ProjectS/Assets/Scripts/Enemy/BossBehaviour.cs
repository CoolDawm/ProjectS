using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BossBehaviour : MonoBehaviour, IEnemy, IEnemyMovement,IEnemyAbilitiesUse
{
    [SerializeField]
    protected float _detectionRadius = 20f;
    [SerializeField]
    private float _aggroRadius = 10f;
    [SerializeField]
    protected float _attackRange = 6f;
    [SerializeField]
    protected float _abilityCooldown = 0f;
    protected GameObject _player;
    protected bool _isAggro = false;

    // Implementing properties from IEnemy
    public float detectionRadius { get { return _detectionRadius; } }
    public float aggroRadius { get { return _aggroRadius; } }
    public float attackRange { get { return _attackRange; } }
    public float abilityCooldown { get { return _abilityCooldown; } }
    // Implementing properties from IEnemyMovement
    public bool isAggro { get { return _isAggro; } }

    protected virtual void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    protected virtual void FixedUpdate()
    {
        // Implement your physics-related logic here
    }
    public abstract void UseAbility();
    public abstract void Attack();
    public abstract void Idle();
    public abstract void ChasePlayer();
}
