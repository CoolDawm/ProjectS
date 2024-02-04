using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
[CreateAssetMenu]
public class RangeAOEV2 : Ability
{
    public GameObject cursorPrefab;
    public float areaOfEffectRadius=5f;
    public float damage = 60;
    private Vector3 _zonePosition;
    private GameObject _cursorObject;
   public override void Activate(GameObject user,CoroutineRunner coroutineRunner)
   {
       abilityIsActive = true;
       if (user.CompareTag("Enemy"))
       {
           GameObject plr = GameObject.FindGameObjectWithTag("Player");
           _zonePosition = plr.transform.position; 
           _cursorObject=  Instantiate(cursorPrefab,_zonePosition,Quaternion.identity);
           coroutineRunner.StartCoroutineFunction(EnemAoeAbilityCoroutine(plr,damage));
       }
       else
       {
           
           _zonePosition = user.transform.position + user.transform.forward*range; 
           _cursorObject=  Instantiate(cursorPrefab,_zonePosition,Quaternion.identity);
           coroutineRunner.StartCoroutineFunction(AoeAbilityCoroutine(user,damage));
       }
   }
   IEnumerator AoeAbilityCoroutine(GameObject user,float aoeDamage)
    {
        
        Debug.DrawRay(_zonePosition, Vector3.up, Color.red, 5f); //
        yield return new WaitForSeconds(1.1f);
        while (abilityIsActive)
        {
            Collider[] colliders = Physics.OverlapSphere(_cursorObject.transform.position, areaOfEffectRadius);
            
            foreach (Collider collider in colliders)
            {
                if (collider.transform.root.gameObject.CompareTag(aim))
                {
                    //collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage);
                    collider.transform.root.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage,Color.yellow);
                }
            }
            abilityIsActive = false;
            yield return null;
        }
        Destroy(_cursorObject);
    }
   IEnumerator EnemAoeAbilityCoroutine(GameObject enemy,float aoeDamage)
   {
        
       Debug.DrawRay(_zonePosition, Vector3.up, Color.red, 5f); //
       yield return new WaitForSeconds(1.1f);
       while (abilityIsActive)
       {
           Collider[] colliders = Physics.OverlapSphere(_cursorObject. transform.position, areaOfEffectRadius);
            
           foreach (Collider collider in colliders)
           {
                            
               if (collider.CompareTag(aim))
               {
                   collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage,Color.red);
               }
           }
           abilityIsActive = false;
           yield return null;
       }
       Destroy(_cursorObject);
       // Hide ability area when ability is not active
   }
}
/*
 * IEnumerator AoeAbilityCoroutine(float aoeDamage)
    {
        Cursor.lockState = CursorLockMode.None;
        freeLook.GetComponent<CinemachineInputProvider>().enabled = false;
        GameObject cursorObject=  Instantiate(cursorPrefab);
        while (abilityIsActive)
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
                        abilityIsActive = false;
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
                            abilityIsActive = false;
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
                            abilityIsActive = false;
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
 */