using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MeleeEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f; // Радиус обнаружения игрока
    [SerializeField] private float aggroRadius = 10f; // Радиус агро
    [SerializeField] private float attackRange = 2f;
    private GameObject player; // Ссылка на игрока
    private NavMeshAgent agent; // Компонент NavMeshAgent для перемещения врага
    private bool isAggro = false; // Флаг, указывающий, атакует ли враг игрока
    private float attackCooldown = 0f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Проверяем, находится ли игрок в зоне обнаружения врага
        if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            // Если игрок находится в зоне агро и враг не атакует, начинаем преследование
            isAggro = true;



            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                StopAllCoroutines();
                attackCooldown += Time.deltaTime;
                if (attackCooldown > 3f)
                {
                    attackCooldown = 0;
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
            isAggro = false;
            StopCoroutine(ChasePlayer());
        }
    }
    public void Attack()
    {
        Debug.Log("Attack");
    }
    IEnumerator ChasePlayer()
    {
        while (isAggro)
        {
            agent.SetDestination(player.transform.position);
            yield return null;
        }
    }
}
