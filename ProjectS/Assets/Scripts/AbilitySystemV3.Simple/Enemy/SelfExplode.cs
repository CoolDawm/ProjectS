using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SelfExplode : Ability
{
    public GameObject cursorPrefab;
    public float areaOfEffectRadius = 5f;
    public float damage = 60;
    private GameObject _cursorObject;
    private Vector3 _zonePosition;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        _zonePosition = user.transform.position;
        coroutineRunner.StartCoroutineFunction(EnemExplodeAbilityCoroutine(user, damage));
    }

    IEnumerator EnemExplodeAbilityCoroutine(GameObject user, float aoeDamage)
    {
        yield return new WaitForSeconds(1f);
        _cursorObject = Instantiate(cursorPrefab, _zonePosition, Quaternion.identity);
        yield return new WaitForSeconds(0.2f);
        Debug.DrawRay(_zonePosition, Vector3.up, Color.red, 5f);
        Collider[] colliders = Physics.OverlapSphere(_cursorObject.transform.position, areaOfEffectRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(aim)&& collider is BoxCollider)
            {
                collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage,Color.red);
            }
        }
        Destroy(_cursorObject);
        Destroy(user);
        abilityIsActive = false;
        yield return null;
        // Hide ability area when ability is not active
        
    }
}
