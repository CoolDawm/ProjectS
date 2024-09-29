using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class EnemyMelee : Ability
{
    public float meleeDamage;
    public float meleeRange;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner, Animator animator)
    {
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(EnemMeleeCoroutine(user, animator));
    }

    IEnumerator EnemMeleeCoroutine(GameObject user, Animator animator)
    {
        animator.SetTrigger(animName);
        yield return new WaitForSeconds(animTime);
        if (user == null)
        {
            abilityIsActive = false;

            yield break;
        }
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, meleeRange);
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            if (Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {

                if (collider.CompareTag(aim)&&collider is BoxCollider)
                {
                    Debug.Log("Punch");
                    Debug.Log(user);
                    collider.GetComponent<HealthSystem>().TakeDamage(meleeDamage,Color.red);
                }
                else if (collider.CompareTag("Summon")&&collider is BoxCollider)
                {
                    collider.GetComponent<HealthSystem>().TakeDamage(meleeDamage,Color.red);

                }
                
            }
        }

        abilityIsActive = false;
    }
}
