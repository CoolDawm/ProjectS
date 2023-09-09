using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
public class SummonerEnemyBehaviour : MonoBehaviour
{
    [SerializeField]
    public float _detectionRadius = 20f;
    [SerializeField]
    public float _aggroRadius = 20f;
    [SerializeField]
    private float _attackRange = 15f;
    [SerializeField]
    private float _rotationSpeed = 6f;
    private float _distance = 5f;
    private GameObject _player;
    private NavMeshAgent _agent;
    public GameObject summonPrefab;
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
                Vector3 targetDirection = _player.transform.position - transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * _rotationSpeed);
                if (_attackCooldown > 8f)
                {
                    _attackCooldown = 0;
                    Summon();
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
    public void Summon()
    {
        Debug.Log("Attack");

        GameObject projectile = Instantiate(summonPrefab, Random.insideUnitCircle * _distance, Quaternion.identity );
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
