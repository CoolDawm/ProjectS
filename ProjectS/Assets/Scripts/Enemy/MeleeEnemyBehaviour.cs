using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MeleeEnemyBehaviour : MonoBehaviour
{
    [SerializeField] 
    private float _detectionRadius = 10f; 
    [SerializeField] 
    private float _aggroRadius = 10f; 
    [SerializeField] 
    private float _attackRange = 4f;
    [SerializeField]
    private float _damage = 10f;
    private GameObject _player; 
    private NavMeshAgent _agent;
    private bool _isAggro = false; 
    private float _attackCooldown = 0f;
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
                _attackCooldown += Time.deltaTime;
                if (_attackCooldown > 5f)
                {
                    _attackCooldown = 0;
                    Attack();
                }
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
