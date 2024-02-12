using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu]
public class SimpleRangeV2 : Ability
{
    [SerializeField] 
    private GameObject projectilePrefab;
    [SerializeField] 
    private float projectileSpeed = 100f;
    private GameObject _shootingPosition;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        if (_shootingPosition == null)
        {
            _shootingPosition = user.GetComponentsInChildren<Transform>()
                .FirstOrDefault(c => c.gameObject.name == "ShootingPosition")?.gameObject;
        }
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, range);
        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            float distanceToTarget = directionToTarget.magnitude;
            
            if (distanceToTarget < closestDistance && Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {
                if (collider.transform.root.CompareTag(aim) && collider is BoxCollider)
                {
                    closestCollider = collider;
                    closestDistance = distanceToTarget;
                }
            }
        }
        GameObject projectile = Instantiate(projectilePrefab, _shootingPosition.transform.position, Quaternion.identity);
        ProjectileScript prScr = projectile.GetComponentInChildren<ProjectileScript>();
        if (closestCollider != null)
        {
            
            prScr.aim = aim;
            prScr.range = range;
            Vector3 direction = (closestCollider.transform.position - _shootingPosition.transform.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            projectile.transform.Rotate(90, 0, 0);
            projectile.GetComponentInChildren<Rigidbody>().velocity = direction * projectileSpeed;
        }
        else
        {
            prScr.aim = aim;
            prScr.range = range;
            Vector3 direction = user.transform.forward;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            projectile.transform.Rotate(90, 0, 0);
            projectile.GetComponentInChildren<Rigidbody>().velocity = direction * projectileSpeed;
        }
    }
    
}
