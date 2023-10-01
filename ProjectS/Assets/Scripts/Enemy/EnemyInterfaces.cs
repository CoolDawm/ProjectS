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
public interface IEnemyAbilitiesUse
{
    float abilityCooldown { get; }
    void UseAbility();
}
public interface IEnemyMovement
{
    bool isAggro { get; }
    void ChasePlayer();
    void Idle();
}


