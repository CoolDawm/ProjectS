using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SimpleRangeV2 : Ability
{
    public GameObject projectilePrefab;
    public float projectileLifeTime = 5f;
    public float projectileSpeed = 100f;
    private Transform shootingPosition;
    public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
    {
        if (shootingPosition == null)
        {
            shootingPosition = user.transform.Find("ShootingPosition");
        }
        GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = shootingPosition.transform.forward * projectileSpeed;
        Destroy(projectile, projectileLifeTime);
    }
}
