using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerBattleSystem : MonoBehaviour
{
    [SerializeField]
    private Transform shootingPosition;
    [SerializeField]
    private float aoeDamage=5;
    [SerializeField]
    private float meleeDamage = 25;
    [SerializeField]
    private bool _abilityIsActive = true;
    public GameObject cursorObject;
    public float manaGenerationRate;
    public float meleeAbilityRange;
    public float rangedAbilityProjectileLifetime;
    public float areaAbilityRadius;
    public float areaAbilityRange;
    private bool isUsingAbility;
    private float manaTimer;
    public float meleeRange = 3f;
    public float projectileSpeed = 1500f;
    public float projectileLifetime = 3f;
    public float areaOfEffectRadius = 5f;
    public float summonRange = 10f;
    public GameObject projectilePrefab;
  
    void Start()
    {
        manaTimer = manaGenerationRate;
    }


    private void GenerateMana()
    {
        
    }


    private void Update()
    {
        GenerateMana();
        //(Melee)
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
           
            Collider[] colliders = Physics.OverlapSphere(transform.position, meleeRange);

            
            foreach (Collider collider in colliders)
            {
                Debug.Log("try");
                if (collider.CompareTag("Enemy"))
                {
                   
                   collider.gameObject.GetComponent<HealthSystem>().TakeDamage(meleeDamage);
                }
            }
        }

        //  (Range)
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = transform.forward * projectileSpeed;

            Destroy(projectile, projectileLifetime);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _abilityIsActive = true;
            StartCoroutine(AoeAbility());
        }
        // Ability 4
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            
        }


    }
    // AOE ability
    IEnumerator AoeAbility()
    {
        CinemachineFreeLook cinemachineFreeLook = Camera.main.GetComponent<CinemachineFreeLook>();
        
        float originalXYAxisValue = cinemachineFreeLook.m_XAxis.m_InputAxisValue;
        float originalYAxisValue = cinemachineFreeLook.m_YAxis.m_InputAxisValue;

        cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0f;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0f;
        
        while (_abilityIsActive)
        {
           
            // Ray from camera to cursor
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                // If ray lands on enemy, show it
                if (hit.collider.CompareTag("Enemy"))
                {
                    cursorObject.transform.position = hit.collider.transform.position;
                    ShowAbilityArea(cursorObject);
                    hit.collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                    if (Input.GetMouseButtonDown(0))
                    {
                        //Attack
                        _abilityIsActive = false;
                    }
                }
                else
                {
                    // If not, show where cursor lands at the end
                    cursorObject.transform.position = hit.point;
                    ShowAbilityArea(cursorObject);

                    // Overlapsphere to find enemies within the area of effect
                    Collider[] colliders = Physics.OverlapSphere(cursorObject.transform.position, areaOfEffectRadius, LayerMask.GetMask("Enemy"));
                    if (colliders.Length > 0)
                    {
                        // Find the closest enemy to the center of the overlapsphere
                        Vector3 centerOfSphere = cursorObject.transform.position;
                        Collider closestEnemy = colliders[0];
                        float closestDistance = Vector3.Distance(centerOfSphere, closestEnemy.transform.position);

                        foreach (Collider collider in colliders)
                        {
                            float distance = Vector3.Distance(centerOfSphere, collider.transform.position);
                            if (distance < closestDistance)
                            {
                                closestEnemy = collider;
                                closestDistance = distance;
                            }
                        }

                        // Move cursor object towards the closest enemy
                        cursorObject.transform.position = closestEnemy.transform.position;
                        if (Input.GetMouseButtonDown(0))
                        {
                            //Attack
                            _abilityIsActive = false;
                        }
                    }
                }
            }

            yield return null;
        }
        
        cinemachineFreeLook.m_XAxis.m_InputAxisValue = originalXYAxisValue;
        cinemachineFreeLook.m_YAxis.m_InputAxisValue = originalYAxisValue;
        // Hide ability area when ability is not active
        HideAbilityArea(cursorObject);
    }
    private void ShowAbilityArea(GameObject point)
    {
        // Show area? Probably will use particles for that
        
    }
    private void HideAbilityArea(GameObject point)
    {
        // Show area? Probably will use particles for that

    }
}

