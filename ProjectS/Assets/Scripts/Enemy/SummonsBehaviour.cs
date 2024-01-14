using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonsBehaviour : EnemyBehaviour
{
    private NavMeshAgent _agent;
    protected override void Start()
    {
        base.Start();
        _agent = GetComponent<NavMeshAgent>();
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
            if (Vector3.Distance(transform.position, _player.transform.position) <=_characteristics.charDic["attackRange"])
            {
                Idle();
                _agent.SetDestination(transform.position);          
                Attack();
                Destroy(gameObject);
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
        _agent.speed = _characteristics.charDic["movementSpeed"];
        _agent.SetDestination(_player.transform.position);
    }
    public override void Idle()
    {
        StopAllCoroutines();
    }
    public override void Attack()
    {
        _player.GetComponent<HealthSystem>().TakeDamage(_characteristics.charDic["damage"]);
        Die();
        Debug.Log("Boom");
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
    public override void Die()
    {
        Destroy(gameObject);
    }
}