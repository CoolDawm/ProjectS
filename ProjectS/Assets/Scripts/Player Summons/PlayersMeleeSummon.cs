using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayersMeleeSummon : PlayersSummonBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _characteristics = GetComponent<Characteristics>();
        _animator = GetComponentInChildren<Animator>();
        _healthSystem = GetComponent<HealthSystem>();
        _healthSystem.OnDeath += Die;
        _healthSystem.OnTakeDamage+=TakeDamageAnim;
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        isAgroed = false;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (_player == null)
        {
            return;
        }
        navMeshAgent.speed = _characteristics.secondCharDic["MovementSpeed"];
        if (navMeshAgent.hasPath)
        {
            _animator.SetBool("Walk Forward", true);
        }
        else
        {
            _animator.SetBool("Walk Forward", false);

        }
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (!isAgroed)
        {
            Collider[] enemies = Physics.OverlapSphere(transform.position, _attackDistance);
            Collider closestCollider = null;
            float closestDistance = _attackDistance;
            foreach (Collider enem in enemies)
            {
                 _directionToTarget = enem.transform.position - gameObject.transform.position;
                 _distanceToTarget = _directionToTarget.magnitude;
                if (enem.CompareTag("Enemy")&&_distanceToTarget < closestDistance)
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
                _distance = Vector3.Distance(transform.position, _enemy.transform.position);
                _isUsingAb = CheckForUsing();
                _attackCooldown += Time.deltaTime;
                if (_distance<=_abilityList[0].range-2)
                {
                    navMeshAgent.SetDestination(transform.position);
                    Vector3 targetDirection = _enemy.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 6);
                    if ( _attackCooldown > 2f && !_isUsingAb)
                    {
                        _attackCooldown = 0;
                        Attack();
                    }
                
                }
                else
                {
                    Vector3 targetDirection = _enemy.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    transform.rotation =
                        Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 6);
                    navMeshAgent.SetDestination(_enemy.position);
                }
            }
            else
            {
                isAgroed = false;
            }
            
            
           
        }
        if (distanceToPlayer >= _followDistance&&!isAgroed)
        {
            Vector3 targetDirection = _player.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation =
                Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 6);
            navMeshAgent.SetDestination(_player.position);
        }
        else if(distanceToPlayer <= _followDistance&&!isAgroed)
        {
            navMeshAgent.SetDestination(transform.position);
        }
    }
    private void TakeDamageAnim()
    {
        _animator.SetTrigger("Take Damage");
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
    public override void Die()
    {
        Destroy(gameObject);
    }
    public override void Attack()
    {
        _abilityList[0].Activate(gameObject,_coroutineRunner,_animator);
    }
}
