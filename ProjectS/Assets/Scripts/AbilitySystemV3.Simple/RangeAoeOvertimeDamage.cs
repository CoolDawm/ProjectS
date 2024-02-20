using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class RangeAoeOvertimeDamage :Ability
{
    [SerializeField] 
    private int _ticksAmount = 0;
    [SerializeField] 
    private float _timeBetweenTicks = 0;
    public GameObject cursorPrefab;
    public float areaOfEffectRadius=5f;
    public float damage = 60;
    private Vector3 _zonePosition;
    private GameObject _cursorObject;
   
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, range);
        Collider closestCollider = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            float distanceToTarget = directionToTarget.magnitude;
            
            if (collider.CompareTag(aim)&&distanceToTarget < closestDistance && Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {
                if (collider.transform.root.CompareTag(aim) && collider is BoxCollider)
                {
                    closestCollider = collider;
                    closestDistance = distanceToTarget;
                }
            }
        }
        if (closestCollider != null)
        {
            _zonePosition = closestCollider.transform.position;
             coroutineRunner.StartCoroutineFunction(EnemAoeAbilityCoroutine(_cursorObject, damage));
        }
        
        
    }
    IEnumerator EnemAoeAbilityCoroutine(GameObject enemy,float aoeDamage)
    {
        int ticksCount = 0;
        Collider[] colliders;
        _cursorObject = Instantiate(cursorPrefab, _zonePosition, Quaternion.identity);
        Debug.DrawRay(_zonePosition, Vector3.up, Color.red, 5f); 
        while (abilityIsActive)
        {
            colliders = Physics.OverlapSphere(_cursorObject.transform.position, areaOfEffectRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(aim))
                {
                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage,Color.red);
                }
            }
            yield return new WaitForSeconds(_timeBetweenTicks);
            ticksCount++;
            if (ticksCount >= _ticksAmount)
            {
                abilityIsActive = false;
            }
            yield return null;
        }
        Destroy(_cursorObject);
        // Hide ability area when ability is not active
    }
}

