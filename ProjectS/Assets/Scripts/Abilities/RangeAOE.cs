using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class RangeAOE : MonoBehaviour
{
    public GameObject cursorObject;
    public float areaOfEffectRadius;
    private bool _abilityIsActive = false; 
    private GameObject freeLook; 
    public void Start()
    {    
        freeLook = GameObject.FindGameObjectWithTag("FreeLookCamera");
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
                            
                            if (collider.CompareTag("Enemy"))
                            {

                                collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                            }
                        }
                        
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
                                
                                if (collider.CompareTag("Enemy"))
                                {
                                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                                }
                            }
                           
                            //Attack
                            _abilityIsActive = false;
                        }
                    }
                    else
                    {
                        if (Input.GetMouseButtonDown(0))
                        {
                            Collider[] colliders1 = Physics.OverlapSphere(cursorObject.transform.position, areaOfEffectRadius);
                            foreach (Collider collider in colliders)
                            {
                                
                                if (collider.CompareTag("Enemy"))
                                {
                                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                                }
                            }
                           
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
}
