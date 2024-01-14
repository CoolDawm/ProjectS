using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class MeleeEnemyBehaviour : EnemyBehaviour
{
    [SerializeField]
    private float _attackCooldown =0;
    protected override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        _characteristics=gameObject.GetComponent<Characteristics>(); 
        HealthSystem healthSystem = GetComponent<HealthSystem>();
        healthSystem.OnDeath += Die;
        _attackRange = 2f;
    }

    protected override void FixedUpdate()
    {
        if (_player == null)
        {
            return;
        }
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {
            Debug.Log(Vector3.Distance(transform.position, _player.transform.position)+" <="+_detectionRadius);
            _isAggro = true;
            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                Idle();
                agent.SetDestination(transform.position);
                
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
            Patrool();
        }
    }
    public override void ChasePlayer()
    {
        agent.speed = _characteristics.secondCharDic["MovementSpeed"];
        agent.SetDestination(_player.transform.position);
    }

    public override void Patrool()
    {
        changePositionTimer += Time.deltaTime;
        Debug.Log(changePositionTimer);
        if (changePositionTimer >= 6f)
        {
            if(agent.remainingDistance <= agent.stoppingDistance) //done with path
            {
                Vector3 point;
                if (RandomPoint(transform.position, range, out point)) //pass in our centre point and radius of area
                {
                    Debug.DrawRay(point, Vector3.up, Color.red, 5f); //gizmos
                    agent.SetDestination(point);
                    changePositionTimer = 0f;
                }
            }
        }
        
    }
    public override void Idle()
    {
        StopAllCoroutines();
    }
   
    public override void Attack()
    {
        _player.GetComponent<HealthSystem>().TakeDamage(_characteristics.charDic["Strength"]*2);
    }

    public override void Die()
    {
        Destroy(gameObject);
    }
    
}
