using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [SerializeField] 
    private float _damage = 15;
    public string aim;
    public float range;
    private Vector3 _initialPosition;
    
    void Start()
    {
        _initialPosition = transform.position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (aim == "Enemy")
        {
            if (collision.collider.transform.root.CompareTag(aim))
            {
                    collision.collider.transform.root.gameObject.GetComponent<HealthSystem>().TakeDamage(_damage,Color.yellow);
                    Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            Debug.Log(collision.collider.name);
            Debug.Log(collision.collider.tag);
            if (collision.collider.CompareTag(aim))
            {
                collision.collider.gameObject.GetComponent<HealthSystem>().TakeDamage(_damage,Color.red);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
    }

    void Update()
    {
        float distanceTraveled = Vector3.Distance(_initialPosition, transform.position);
        if (distanceTraveled > range)
        {
            Destroy(gameObject);
        }
    }
}
