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
