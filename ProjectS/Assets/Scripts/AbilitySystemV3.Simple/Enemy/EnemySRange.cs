using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class EnemySRange : Ability
{
    [SerializeField]
    private GameObject _fxEffect;
    [SerializeField]
    private GameObject projectilePrefab;
    [SerializeField]
    private float projectileSpeed = 100f;
    private Transform shootingPosition;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner, Animator animator)
    {
        if (shootingPosition == null)
        {
            shootingPosition = user.transform.Find("ShootingPosition");
        }
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(RangeCoroutitine(user, animator));
    }
    IEnumerator RangeCoroutitine(GameObject user, Animator animator)
    {
        animator.SetTrigger(animName);
        yield return new WaitForSeconds(animTime);
        if (shootingPosition == null)
        {
            yield break;
        }
        GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
        ProjectileScript prScr = projectile.GetComponent<ProjectileScript>();
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = user.transform.forward * projectileSpeed;
        prScr.aim = "Player";
        prScr.range = 15;
        if (_fxEffect != null)
        {
            Instantiate(_fxEffect, projectile.transform.position, Quaternion.identity, projectile.transform);
        }
        abilityIsActive = false;
    }
}