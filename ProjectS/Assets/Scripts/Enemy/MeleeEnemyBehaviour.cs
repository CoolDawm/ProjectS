using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MeleeEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 10f; // ������ ����������� ������
    [SerializeField] private float _aggroRadius = 10f; // ������ ����
    [SerializeField] private float _attackRange = 4f;
    private GameObject _player; // ������ �� ������
    private NavMeshAgent _agent; // ��������� NavMeshAgent ��� ����������� �����
    private bool _isAggro = false; // ����, �����������, ������� �� ���� ������
    private float _attackCooldown = 0f;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // ���������, ��������� �� ����� � ���� ����������� �����
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {
            // ���� ����� ��������� � ���� ���� � ���� �� �������, �������� �������������
            _isAggro = true;



            if (Vector3.Distance(transform.position, _player.transform.position) <= _attackRange)
            {
                StopAllCoroutines();
                _agent.SetDestination(transform.position);
                _attackCooldown += Time.deltaTime;
                if (_attackCooldown > 3f)
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
