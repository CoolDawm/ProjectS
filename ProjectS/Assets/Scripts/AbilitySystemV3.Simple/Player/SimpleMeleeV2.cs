using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
//Remove this script from meleeboss and then rework it into simlier version
public class SimpleMeleeV2 : Ability
{
    public float meleeDamage;
    public float meleeRange;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        //suser.transform.rotation = lookRotation;
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, meleeRange);
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            if (Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {
                if (collider.transform.root.CompareTag(aim) && collider is BoxCollider)
                {
                    collider.transform.root.GetComponent<HealthSystem>().TakeDamage(meleeDamage, Color.white);
                    user.GetComponent<AbilityHolder>().GenerateMana(meleeDamage / 5);
                }
            }
        }

        abilityIsActive = false;
    }
}
