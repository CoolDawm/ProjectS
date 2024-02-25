using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]

public class RangeAOEAutoAim : Ability
{
    public GameObject cursorPrefab;
    public float areaOfEffectRadius=5f;
    public float damage = 60;
    private Vector3 _zonePosition;
    private GameObject _cursorObject;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner,Animator animator)
    {
        abilityIsActive = true;

        GameObject plr = GameObject.FindGameObjectWithTag("Player");
        _zonePosition = plr.transform.position;
        _cursorObject = Instantiate(cursorPrefab, _zonePosition, Quaternion.identity);
        animator.SetTrigger(animName);
        coroutineRunner.StartCoroutineFunction(EnemAoeAbilityCoroutine(plr, damage));
        
    }
   IEnumerator EnemAoeAbilityCoroutine(GameObject enemy,float aoeDamage)
   {
       yield return new WaitForSeconds(animTime);
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

