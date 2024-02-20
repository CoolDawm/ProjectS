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
    [SerializeField]
    private float fieldOfViewAngle = 10f; 
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
        float closestDistance = range;
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            float distanceToTarget = directionToTarget.magnitude;
            
            if (distanceToTarget < closestDistance && Vector3.Angle(user.transform.forward, directionToTarget) < fieldOfViewAngle)
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
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Ray ray = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;
            prScr.aim = aim;
            prScr.range = range;
            Vector3 direction = (targetPosition - _shootingPosition.transform.position).normalized;
            projectile.transform.rotation = Quaternion.LookRotation(direction);
            projectile.transform.Rotate(90, 0, 0);
            projectile.GetComponentInChildren<Rigidbody>().velocity = direction * projectileSpeed;

        }
        /*
        if (closestCollider != null)
        {
            user.GetComponent<AbilityHolder>().GenerateMana(3);
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
        }*/
    }
    
}
