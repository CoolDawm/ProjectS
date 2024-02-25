using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class HealAura : Ability
{
    [SerializeField] 
    private int _ticksAmount = 0;
    [SerializeField] 
    private float _timeBetweenTicks = 0;
    public float areaOfEffectRadius=5f;
    public float healAmount = 5;
    private Vector3 _zonePosition;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        
             coroutineRunner.StartCoroutineFunction(EnemAoeAbilityCoroutine(user));
    }
    IEnumerator EnemAoeAbilityCoroutine(GameObject user)
    {
        int ticksCount = 0;
        Collider[] colliders;
        Debug.DrawRay(_zonePosition, Vector3.up, Color.red, 5f); 
        while (abilityIsActive)
        {
            colliders = Physics.OverlapSphere(user.transform.position, areaOfEffectRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(aim))
                {
                    collider.gameObject.GetComponent<HealthSystem>().IncreaseCurrentHealth(healAmount);
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
    }
}
