using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainProjectile : MonoBehaviour
{
    [SerializeField]
    private float _damage = 15;
    [SerializeField]
    private float _projectileSpeed = 20;
    [SerializeField]
    private GameObject _chainLightningPrefab;
    public string aim { private get; set; }
    public float range;
    private Vector3 _initialPosition;
    private Vector3 _offsetVector = new Vector3(0, 0.5f,0);
    void Start()
    {
        _initialPosition = transform.position;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.root.CompareTag(aim))
        {
            CreateChainLightning(collision.gameObject);
            collision.collider.transform.root.gameObject.GetComponent<HealthSystem>().TakeDamage(_damage, Color.yellow);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
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
    
    private void CreateChainLightning(GameObject enemyPoint)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject projectile = Instantiate(_chainLightningPrefab, enemyPoint.transform.position - enemyPoint.transform.forward * (i + 3)+_offsetVector, Quaternion.identity);
            ChainProjectile prScr = projectile.GetComponent<ChainProjectile>();
            prScr.aim = aim;
            prScr.range = 6;
            float angle = Random.Range(-40f, 40f);
            Vector3 direction = Quaternion.Euler(0, angle, 0) *projectile.transform.forward;
            projectile.GetComponent<Rigidbody>().velocity = direction.normalized * _projectileSpeed;
        }
    }
}
