using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRange : MonoBehaviour
{
    public GameObject projectilePrefab;
    // Start is called before the first frame update
    void Start()
    {
        projectilePrefab=Resources.Load<GameObject>("Prefabs/Projectiles/Player_Projectile01 1");
    }

    public void Shoot(float projectileLifeTime, float projectileSpeed, Transform shootingPosition)
    {
        GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.velocity = shootingPosition.transform.forward * projectileSpeed;
        Destroy(projectile, projectileLifeTime);
    }
}
