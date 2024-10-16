using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class MeleeAoeOvertimeDamage : Ability
{
    [SerializeField]
    private int _ticksAmount = 0;
    [SerializeField]
    private float _timeBetweenTicks = 0;
    [SerializeField]
    private GameObject _aoePrefab;
    public GameObject _aoeEffect;
    public float areaOfEffectRadius = 5f;
    public float damage = 10;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;


        coroutineRunner.StartCoroutineFunction(EnemAoeAbilityCoroutine(user, damage));
    }
    IEnumerator EnemAoeAbilityCoroutine(GameObject user, float aoeDamage)
    {
        int ticksCount = 0;
        Collider[] colliders;
        _aoeEffect = Instantiate(_aoePrefab, user.transform.position, Quaternion.identity,user.transform);
        Debug.DrawRay(user.transform.position, Vector3.up, Color.red, 5f);
        while (abilityIsActive)
        {
            colliders = Physics.OverlapSphere(user.transform.position, areaOfEffectRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(aim) && !collider.isTrigger)
                {
                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(aoeDamage, Color.red);
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
        Destroy(_aoeEffect);
        // Hide ability area when ability is not active
    }
}
