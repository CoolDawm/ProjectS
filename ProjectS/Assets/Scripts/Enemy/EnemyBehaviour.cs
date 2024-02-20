using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public abstract class EnemyBehaviour : MonoBehaviour, IEnemy, IEnemyMovement
{
    [SerializeField]
    protected float _detectionRadius = 6f;
    [SerializeField]
    private float _aggroRadius = 6f;
    [SerializeField]
    protected NavMeshAgent agent;
    [SerializeField]
    protected float _attackRange = 4f;
    public float range=10f; //radius of sphere
    protected float changePositionTimer = 0;
   
    protected Characteristics _characteristics; 
    protected GameObject _target;
    protected bool _isAggro = false;
    protected Animator _animator;
    // Implementing properties from IEnemy
    public float detectionRadius { get { return _detectionRadius; } }
    public float aggroRadius { get { return _aggroRadius; } }

    // Implementing properties from IEnemyMovement
    public bool isAggro
    {
        get { return _isAggro; }
        set => _isAggro = value;
    }

    protected virtual void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        _animator = GetComponentInChildren<Animator>();
    }
    protected virtual void Update()
    {
        // Implement your physics-related logic here
    }

    public abstract void Die();
    public abstract void Attack();
    public abstract void Idle();
    public abstract void ChasePlayer();
    public abstract void Patrool();
    public abstract void Agroed();
    public abstract void ChangeTarget();
    public abstract void TakeDamageAnim();
    protected bool RandomPoint(Vector3 center, float range, out Vector3 result)// Redo it in Utils
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in a sphere 
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1f, NavMesh.AllAreas)) //navmesh doc https://docs.unity3d.com/ScriptReference/AI.NavMesh.SamplePosition.html
        { 
            //the 1.0f is the max distance from the random point to a point on the navmesh, might want to increase if range is big
            result = hit.position;
            return true;
        }
        result = Vector3.zero;
        return false;
    }
}
