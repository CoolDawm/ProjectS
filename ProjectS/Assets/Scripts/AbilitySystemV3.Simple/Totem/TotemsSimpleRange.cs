using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class TotemsSimpleRange : Ability
{
    [SerializeField] 
    private GameObject projectilePrefab;
    [SerializeField] 
    private float projectileSpeed = 100f;
    private Transform shootingPosition;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner,GameObject target)
    {
        if (shootingPosition == null)
        {
            shootingPosition = user.transform.Find("ShootingPosition");
        }
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(RangeCoroutitine(target));
    }
    IEnumerator RangeCoroutitine(GameObject target)
    {
        Vector3 direction = (target.transform.position - shootingPosition.transform.position).normalized;
        if (shootingPosition == null)
        {
            yield break;
        }
        GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
        ProjectileScript prScr = projectile.GetComponent<ProjectileScript>();
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = direction * projectileSpeed;
        prScr.aim = aim;
        prScr.range = range;
        
        abilityIsActive = false;
    }
}
