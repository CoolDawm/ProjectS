using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SimpleRangeV2 : Ability
{
    public GameObject projectilePrefab;
    public float projectileSpeed = 100f;
    private Transform shootingPosition;
    public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
   {
       if (shootingPosition == null)
       {
           shootingPosition = user.transform.Find("ShootingPosition");
       }
       Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
       Ray ray = Camera.main.ScreenPointToRay(screenCenter);
       RaycastHit hit;
       if (Physics.Raycast(ray, out hit))
       {
           Vector3 targetPosition = hit.point;
           GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
           projectile.GetComponent<ProjectileScript>().aim = aim;
           projectile.GetComponent<ProjectileScript>().range = range;
           Vector3 direction = (targetPosition - shootingPosition.position).normalized;
           projectile.GetComponent<Rigidbody>().velocity = direction * projectileSpeed;
       }
        /*abilityIsActive = true;
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
                Debug.Log(hitCollider.gameObject.name);
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
            closestEnemy.GetComponent<HealthSystem>().TakeDamage(15f);
            /*
            Vector3 direction = closestEnemy.transform.position - user.transform.position;
            user.transform.rotation = Quaternion.LookRotation(direction);
            GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = shootingPosition.transform.forward * projectileSpeed;
            Destroy(projectile, projectileLifeTime);* /
        }
        else
        {
            Debug.Log("We not in");
            /*
            GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = shootingPosition.transform.forward * projectileSpeed;
            Destroy(projectile, projectileLifeTime);* /
        }
        abilityIsActive = false;*/
    }
}
