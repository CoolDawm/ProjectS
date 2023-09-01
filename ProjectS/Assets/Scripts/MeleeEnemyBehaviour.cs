using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MeleeEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float _detectionRadius = 10f; // Радиус обнаружения игрока
    [SerializeField] private float _aggroRadius = 10f; // Радиус агро
    [SerializeField] private float _attackRange = 4f;
    private GameObject _player; // Ссылка на игрока
    private NavMeshAgent _agent; // Компонент NavMeshAgent для перемещения врага
    private bool _isAggro = false; // Флаг, указывающий, атакует ли враг игрока
    private float _attackCooldown = 0f;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Проверяем, находится ли игрок в зоне обнаружения врага
        if (Vector3.Distance(transform.position, _player.transform.position) <= _detectionRadius)
        {
            // Если игрок находится в зоне агро и враг не атакует, начинаем преследование
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
