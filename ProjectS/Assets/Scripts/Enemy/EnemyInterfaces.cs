using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public interface IEnemy
{
    float detectionRadius { get; }
    float aggroRadius { get; }
    void Attack();
    void Die();
}

public interface IEnemyMovement
{
    void ChasePlayer();
    void Idle();
}


