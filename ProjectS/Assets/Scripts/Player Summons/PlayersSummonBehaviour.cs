using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class PlayersSummonBehaviour : MonoBehaviour
{
    [SerializeField]
    protected Transform _player;
    [SerializeField]
    protected float _followDistance;
    [SerializeField]
    protected float _attackDistance;
    [SerializeField] 
    protected List<Ability> _abilityList;
    protected bool isAgroed;
    protected NavMeshAgent navMeshAgent;
    protected Characteristics _characteristics;
    protected Animator _animator;
    protected HealthSystem _healthSystem;
    protected CoroutineRunner _coroutineRunner;
    protected Transform _enemy;
    protected Vector3 _directionToTarget;
    protected float _distanceToTarget;
    protected float _distance;
    protected float _attackCooldown =0;
    protected bool _isUsingAb=false;

    protected virtual void FixedUpdate()
    {
    }
    public abstract void Attack();
    public abstract void Die();

}
