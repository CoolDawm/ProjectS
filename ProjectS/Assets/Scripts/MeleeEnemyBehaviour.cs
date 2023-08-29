using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class MeleeEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f; // ������ ����������� ������
    [SerializeField] private float aggroRadius = 10f; // ������ ����
    [SerializeField] private float attackRange = 2f;
    private GameObject player; // ������ �� ������
    private NavMeshAgent agent; // ��������� NavMeshAgent ��� ����������� �����
    private bool isAggro = false; // ����, �����������, ������� �� ���� ������
    private float attackCooldown = 0f;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // ���������, ��������� �� ����� � ���� ����������� �����
        if (Vector3.Distance(transform.position, player.transform.position) <= detectionRadius)
        {
            // ���� ����� ��������� � ���� ���� � ���� �� �������, �������� �������������
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
