using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitesManager : MonoBehaviour
{
    [SerializeField]
    private float _bombTimer = 0f;
    private bool _boom;
    public GameObject cursorObject;
    public float areaOfEffectRadius;
    public GameObject projectilePrefab;
    private bool _abilityIsActive = false;
    private GameObject freeLook;
    public void Start()
    {    
        freeLook = GameObject.FindGameObjectWithTag("FreeLookCamera");
    }
    public void FixedUpdate()
    {
        if(freeLook == null)
        {
            freeLook = GameObject.FindGameObjectWithTag("FreeLookCamera");
        }
    }
    public void MeleeAbility(float meleeRange,float meleeDamage,GameObject attacker)
    {
        if (attacker.tag == "Enemy")
        {
            Collider[] colliders = Physics.OverlapSphere(attacker.transform.position, meleeRange);
            foreach (Collider collider in colliders)
            {
                Debug.Log("try");
                if (collider.CompareTag("Player"))
                {

                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(meleeDamage);
                    break;
                }
            }
        }
        else if (attacker.tag == "Player")
        {
            Collider[] colliders = Physics.OverlapSphere(attacker.transform.position, meleeRange);
            foreach (Collider collider in colliders)
            {
                Debug.Log("try");
                if (collider.CompareTag("Enemy"))
                {
                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(meleeDamage);
                }
            }

        }
        
    }

    public void RangeAbility(float currentMana,float projectileLifeTime,float projectileSpeed, Transform shootingPosition)
    {
        Debug.Log(currentMana);
        if (currentMana >= 10)
        {
            currentMana -= 10;
            GameObject projectile = Instantiate(projectilePrefab, shootingPosition.position, Quaternion.identity);
            Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
            projectileRigidbody.velocity = transform.forward * projectileSpeed;
            Destroy(projectile, projectileLifeTime);
        }
    }

    public void AoeAbility(float currentMana,float aoeDamage)
    {
        if (currentMana >= 20)
        {
            _abilityIsActive = true;
            StartCoroutine(AoeAbilityCoroutine(aoeDamage));
        }
    }

    IEnumerator AoeAbilityCoroutine(float aoeDamage)
    {
        freeLook.GetComponent<CinemachineInputProvider>().enabled = false;
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
                        Collider[] colliders = Physics.OverlapSphere(cursorObject.transform.position, areaOfEffectRadius);
                        foreach (Collider collider in colliders)
                        {
                            Debug.Log("try");
                            if (collider.CompareTag("Enemy"))
                            {

                                collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                            }
                        }
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
                            Collider[] colliders1 = Physics.OverlapSphere(cursorObject.transform.position, areaOfEffectRadius);
                            foreach (Collider collider in colliders)
                            {
                                Debug.Log("try");
                                if (collider.CompareTag("Enemy"))
                                {

                                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                                }
                            }
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
        // Hide ability area when ability is not active
        HideAbilityArea(cursorObject);
    }
    private void ShowAbilityArea(GameObject point)
    {
        cursorObject.SetActive(true);

    }
    private void HideAbilityArea(GameObject point)
    {
        cursorObject.SetActive(false);

    }
    public void Shield(float currentMana,GameObject abilityObject)
    {
        abilityObject.GetComponent<HealthSystem>().ShieldCharge(100);
    }
    public void Suicide(GameObject abilityObject)
    {
        StartCoroutine(SuicideAbilityCoroutine(abilityObject));

    }
    IEnumerator SuicideAbilityCoroutine(GameObject abilityObject)
    {
        _boom = true;
        while (_boom == true)
        {
            _bombTimer += Time.deltaTime;
            if (_bombTimer >= 5)
            {
                MeleeAoe(30f, abilityObject);
                Destroy(abilityObject);
                _boom= false;
            }
        }
        yield return null;
    }
    public void MeleeAoe(float damage,GameObject user)
    {
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, areaOfEffectRadius, LayerMask.GetMask("Enemy"));
        foreach (Collider collider in colliders)
        {
            collider.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
        }
    }
}
