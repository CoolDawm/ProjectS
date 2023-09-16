using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEnemy
{
    float detectionRadius { get; }
    float aggroRadius { get; }
    float attackRange { get; }

    void Attack();
}
public interface IEnemyMovement
{
    float detectionRadius { get; }
    bool isAggro { get; }
    void ChasePlayer();
    void Idle();
}


