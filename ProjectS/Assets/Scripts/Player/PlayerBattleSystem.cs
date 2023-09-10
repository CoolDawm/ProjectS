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
    [SerializeField]
    private float _maxMana = 100;
    private GameObject  freeLook;
    public float meleeAbilityRange;
    public float rangedAbilityProjectileLifetime;
    public float areaAbilityRadius;
    public float areaAbilityRange;
    private bool isUsingAbility;
    public float currentMana;
    public GameObject cursorObject;
    public float manaGenerationRate;
    public float meleeRange = 3f;
    public float projectileSpeed = 1500f;
    public float projectileLifetime = 3f;
    public float areaOfEffectRadius = 10f;
    public float summonRange = 10f;
    public GameObject projectilePrefab;
  
    void Start()
    {
        currentMana = _maxMana;
        freeLook = GameObject.FindGameObjectWithTag("FreeLookCamera");
    }


    private void GenerateMana(float mana)
    {
        currentMana += mana;
    }


    private void Update()
    {
        
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
            if (currentMana >= 10)
            {
                currentMana -= 10;
                GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
                Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
                projectileRigidbody.velocity = transform.forward * projectileSpeed;
                Destroy(projectile, projectileLifetime);
            }        
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (currentMana >= 20)
            {
                _abilityIsActive = true;
                StartCoroutine(AoeAbility());
            }
            
        }
        // AOE from player
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, areaOfEffectRadius, LayerMask.GetMask("Enemy"));
            foreach (Collider collider in colliders)
            {
                collider.gameObject.GetComponent<HealthSystem>().TakeDamage(15);
            }
        }


    }
    // AOE ability
    IEnumerator AoeAbility()
    {
        //CinemachineFreeLook cinemachineFreeLook = Camera.main.GetComponent<CinemachineFreeLook>();
        freeLook.GetComponent<CinemachineInputProvider>().enabled = false;
        //float originalXYAxisValue = cinemachineFreeLook.m_XAxis.m_InputAxisValue;
        //float originalYAxisValue = cinemachineFreeLook.m_YAxis.m_InputAxisValue;
        
        //cinemachineFreeLook.m_XAxis.m_InputAxisValue = 0f;
        //cinemachineFreeLook.m_YAxis.m_InputAxisValue = 0f;
        
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
                    
                    if (Input.GetMouseButtonDown(0))
                    {
                        hit.collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                        Debug.Log("Pew Pew Pew Pew Pew");
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

                        
                        if (Input.GetMouseButtonDown(0))
                        {
                            // Move cursor object towards the closest enemy
                            cursorObject.transform.position = closestEnemy.transform.position;

                            Debug.Log("Pew Pew Pew Pew Pew");
                            //Attack
                            _abilityIsActive = false;
                        }
                    }
                }
            }

            yield return null;
        }
        freeLook.GetComponent<CinemachineInputProvider>().enabled = true;
       // cinemachineFreeLook.m_XAxis.m_InputAxisValue = originalXYAxisValue;
        //cinemachineFreeLook.m_YAxis.m_InputAxisValue = originalYAxisValue;
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

