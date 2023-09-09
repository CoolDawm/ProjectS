using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SummonsBehaviour : MonoBehaviour
{
    [SerializeField]
    private float _detectionRadius = 15f;
    [SerializeField]
    private float _aggroRadius = 15f;
    [SerializeField]
    private float _attackRange = 2f;
    [SerializeField]
    private float _damage = 15f;
    private GameObject _player;
    private NavMeshAgent _agent;
    private bool _isAggro = false;
    
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {

        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {

            _isAggro = true;



            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                StopAllCoroutines();
                _agent.SetDestination(transform.position);
                
               
                    Attack();
                
            }
            else
            {
                StartCoroutine(ChasePlayer());
            }
        }
        else
        {
            _isAggro = false;
            StopCoroutine(ChasePlayer());
        }
    }
    public void Attack()
    {
        _player.GetComponent<HealthSystem>().TakeDamage(_damage);
        Debug.Log("Attack");
        //Exsplosion
        Destroy(gameObject);
    }
    IEnumerator ChasePlayer()
    {
        while (_isAggro)
        {
            _agent.SetDestination(_player.transform.position);
            yield return null;
        }
    }
}
