using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TotemBehaviour : MonoBehaviour
{
    [SerializeField] 
    protected List<Ability> _abilityList;
    [SerializeField] 
    protected float _attackDistance;
    [SerializeField] 
    protected float _lifeTime;
    protected CoroutineRunner _coroutineRunner;
    protected float _attackCooldown =0;
    protected bool _isUsingAb=false;
    protected bool isAgroed;
    protected Transform _enemy;
    protected Vector3 _directionToTarget;
    protected float _distanceToTarget;

    protected virtual void FixedUpdate()
    {
    }

    public abstract void Attack(GameObject target);
    public abstract void Attack();
    public abstract void Die();

}
