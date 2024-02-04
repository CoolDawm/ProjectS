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
        Debug.Log("Success");
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, meleeRange);
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            if (Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {

                if (collider.CompareTag(aim))
                {
                    collider.GetComponent<HealthSystem>().TakeDamage(meleeDamage,Color.red);
                }
            }
        }

        abilityIsActive = false;
    }
}
