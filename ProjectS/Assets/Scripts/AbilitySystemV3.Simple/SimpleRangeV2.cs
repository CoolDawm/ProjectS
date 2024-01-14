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
    public float visibilityAngle = 180f;
    public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
   {
       abilityIsActive = true;
        if (shootingPosition == null)
        {
            shootingPosition = user.transform.Find("ShootingPosition");
        }
        Collider[] hitColliders = Physics.OverlapSphere(user.transform.position, range);
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Enemy"))
            {
                Vector3 directionToEnemy = (hitCollider.transform.position - user.transform.position).normalized;
                if (Vector3.Angle(user.transform.forward, directionToEnemy) <= visibilityAngle * 0.5f)
                {
                    float distanceToEnemy = Vector3.Distance(user.transform.position, hitCollider.transform.position);
                    if (distanceToEnemy < closestDistance)
                    {
                        closestDistance = distanceToEnemy;
                        closestEnemy = hitCollider.gameObject;
                    }
                }
            }
        }

        if (closestEnemy != null)
        {
            // Activator rotation
            Debug.Log("We in");
            Vector3 direction = closestEnemy.transform.position - user.transform.position;
            user.transform.rotation = Quaternion.LookRotation(direction);
            GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = shootingPosition.transform.forward * projectileSpeed;
            Destroy(projectile, projectileLifeTime);
        }
        else
        {
            GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = shootingPosition.transform.forward * projectileSpeed;
            Destroy(projectile, projectileLifeTime);
        }
        abilityIsActive = false;
    }
}
