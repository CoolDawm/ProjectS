using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class RangeImpale : Ability
{
    public GameObject cursorPrefab;
    public float areaOfEffectRadius = 3f;
    public float damage = 30;
    private Vector3 _zonePosition;
    private GameObject _cursorObject;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner,Animator animator)
    {
        abilityIsActive = true;
        animator.SetTrigger(animName);
        GameObject plr = GameObject.FindGameObjectWithTag("Player");
        _zonePosition = plr.transform.position;
        _cursorObject = Instantiate(cursorPrefab, _zonePosition, Quaternion.identity);
        coroutineRunner.StartCoroutineFunction(EnemAoeAbilityCoroutine(plr, damage));
    }

    IEnumerator EnemAoeAbilityCoroutine(GameObject enemy, float aoeDamage)
    {
        Debug.DrawRay(_zonePosition, Vector3.up, Color.red, 5f); 
        yield return new WaitForSeconds(0.5f);
        while (abilityIsActive)
        {
            Collider[] colliders = Physics.OverlapSphere(_cursorObject.transform.position, areaOfEffectRadius);

            foreach (Collider collider in colliders)
            {

                if (collider.CompareTag(aim)&& collider is BoxCollider)
                {
                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage, Color.red);
                }
            }

            abilityIsActive = false;
            yield return null;
        }
        Destroy(_cursorObject,3.5f);
    }
}
