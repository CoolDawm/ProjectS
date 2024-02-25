using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraTotem : TotemBehaviour
{
    void Start()
    {
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
    }

    protected override void FixedUpdate()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Die();
        }
        _isUsingAb = CheckForUsing();
        if (!_isUsingAb)
        {
            _attackCooldown = 0;
            Attack();
        }
    }
    //ShitCode-need to fix
    public override void Attack(GameObject target)
    {
        throw new System.NotImplementedException();
    }

    private bool CheckForUsing()
    {
        foreach (Ability ab in _abilityList)
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
        _abilityList[0].Activate(gameObject, _coroutineRunner);
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}