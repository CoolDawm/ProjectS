using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{

    [SerializeField] 
    private float _damage = 15;
    [SerializeField]
    private bool isEnchPr = false;
    public string aim { private get; set; }
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
                if (isEnchPr)
                {
                    Collider[] colliders = Physics.OverlapSphere(collision.transform.position, 3);
                    foreach (Collider collider in colliders)
                    {
                        if (collider.transform.root.gameObject.CompareTag(aim))
                        {
                            //collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                            collider.transform.root.gameObject.GetComponent<HealthSystem>().TakeDamage(_damage,Color.yellow);
                        }
                    }
                }
                else
                {
                    collision.collider.transform.root.gameObject.GetComponent<HealthSystem>()
                        .TakeDamage(_damage, Color.yellow);
                }
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.collider.CompareTag(aim)&& collision.collider is BoxCollider|| collision.collider.CompareTag("Summon")&& collision.collider is BoxCollider)
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
        //proj doesnt delete himself for some reason
        float distanceTraveled = Vector3.Distance(_initialPosition, transform.position);
        if (distanceTraveled > range)
        {
            Destroy(gameObject);
        }
    }
}
