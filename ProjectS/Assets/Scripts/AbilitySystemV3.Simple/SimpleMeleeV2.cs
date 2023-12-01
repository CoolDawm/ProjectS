using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SimpleMeleeV2 : Ability
{
    public float meleeDamage;
    public float meleeRange;
    public string aim;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, meleeRange);
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            if (Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {
                if (collider.CompareTag(aim))
                {
                    collider.gameObject.GetComponent<HealthSystem>().TakeDamage(meleeDamage);
                }
            }
        }

    }


}
