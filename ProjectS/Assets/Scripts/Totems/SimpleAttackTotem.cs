using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleAttackTotem : TotemBehaviour
{
    Collider closestCollider = null;
    // Start is called before the first frame update
    void Start()
    {
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        isAgroed = false;
    }

    protected override void FixedUpdate()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0)
        {
            Die();
        }

        if (!isAgroed)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, _attackDistance);
            closestCollider = null;
            float closestDistance = _attackDistance;
            foreach (Collider enem in enemies)
            {
                _directionToTarget = enem.transform.position - gameObject.transform.position;
                _distanceToTarget = _directionToTarget.magnitude;
                if (enem.CompareTag("Enemy") && _distanceToTarget < closestDistance)
                {
                    closestCollider = enem;
                    closestDistance = _distanceToTarget;
                }
            }

            if (closestCollider != null)
            {
                isAgroed = true;
                _enemy = closestCollider.transform;
            }
        }
        else
        {
            if (_enemy != null)
            {
                _isUsingAb = CheckForUsing();
                _attackCooldown += Time.deltaTime;
                if (_attackCooldown > _abilityList[0].cooldown && !_isUsingAb)
                {
                    _attackCooldown = 0;
                    Attack(closestCollider.gameObject);
                }
            }
            else
            {
                isAgroed = false;
            }
        }
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

    public override void Attack(GameObject target)
    {
        _abilityList[0].Activate(gameObject, _coroutineRunner,target);
    }
    //ShitCode - need to fix
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
}
