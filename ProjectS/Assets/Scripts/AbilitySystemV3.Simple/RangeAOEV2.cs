using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu]
public class RangeAOEV2 : Ability
{
    public GameObject cursorPrefab;
    public float areaOfEffectRadius=10f;
    public float damage = 60;
    private bool _abilityIsActive = false;
    
    private GameObject freeLook;
   public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
   {
       if (freeLook == null)
       {
           freeLook = GameObject.FindGameObjectWithTag("FreeLookCamera");
       }
       _abilityIsActive = true;
       coroutineRunner.StartCoroutineFunction(AoeAbilityCoroutine(damage));
   }
   IEnumerator AoeAbilityCoroutine(float aoeDamage)
    {
        Cursor.lockState = CursorLockMode.None;
        freeLook.GetComponent<CinemachineInputProvider>().enabled = false;
        GameObject cursorObject=  Instantiate(cursorPrefab);
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
                    if (Input.GetMouseButtonDown(0))
                    {
                        Collider[] colliders = Physics.OverlapSphere(cursorObject. transform.position, areaOfEffectRadius);
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
        Destroy(cursorObject);
        Cursor.lockState = CursorLockMode.Locked;
        freeLook.GetComponent<CinemachineInputProvider>().enabled = true;
        // Hide ability area when ability is not active
    }
}
