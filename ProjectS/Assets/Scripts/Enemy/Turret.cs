using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private float[] rotationSpeeds;
    public float projectileSpeed = 10f;
    private List<Transform> partsArr=new List<Transform>();
    public GameObject projectilePrefab;
    private float[] _attackCooldown;
    private float[] _timers;
    
    void Start()
    {
        foreach (Transform child in gameObject.transform.GetChild(0).transform)
        {
            partsArr.Add(child);
        }
        rotationSpeeds = new float[partsArr.Count];
        _attackCooldown = new float[partsArr.Count];
        _timers = new float[partsArr.Count];
        for (int i = 0; i < partsArr.Count; i++)
        {
            rotationSpeeds[i] = Random.Range(15f, 30f); 
            _attackCooldown[i] = Random.Range(1f, 3f);
            _timers[i] = 0;
        }
    }
    protected void FixedUpdate()
    {
        for (int i = 0; i < partsArr.Count; i++)
        {
            partsArr[i].Rotate(new Vector3(0,  rotationSpeeds[i] * Time.deltaTime, 0));
            _timers[i] += Time.deltaTime;
            if (_timers[i] >= _attackCooldown[i])
            {
                Shoot(partsArr[i]);
                _timers[i] = 0;
            }
        }
       
    }
    public void Shoot(Transform obj)
    {
        foreach (Transform child in obj)
        {
            Vector3 shootingDirection = child.position - obj.position;
            GameObject projectile = Instantiate(projectilePrefab, child.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = shootingDirection.normalized * projectileSpeed;
            Destroy(projectile, 3);
        }
       
    }
   
}
