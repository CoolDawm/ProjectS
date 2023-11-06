using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMelee : MonoBehaviour
{
    public void Melee(float meleeRange,float meleeDamage,GameObject attacker)
    {
        if (attacker.CompareTag("Enemy"))
        {
            Collider[] colliders = Physics.OverlapSphere(attacker.transform.position, meleeRange);

            foreach (Collider collider in colliders)
            {
                Vector3 directionToTarget = collider.transform.position - attacker.transform.position;
                if (Vector3.Dot(attacker.transform.forward, directionToTarget) > 0)
                {
                    if (collider.CompareTag("Player"))
                    {

                        collider.gameObject.GetComponent<HealthSystem>().TakeDamage(meleeDamage);
                        break;
                    }
                }
            }
        }
        else if (attacker.CompareTag("Player"))
        {
            Collider[] colliders = Physics.OverlapSphere(attacker.transform.position, meleeRange);
            foreach (Collider collider in colliders)
            {
                Vector3 directionToTarget = collider.transform.position - attacker.transform.position;
                if (Vector3.Dot(attacker.transform.forward, directionToTarget) > 0)
                {
                    if (collider.CompareTag("Enemy"))
                    {
                        collider.gameObject.GetComponent<HealthSystem>().TakeDamage(meleeDamage);
                    }
                }
            }
        }
    }
}
