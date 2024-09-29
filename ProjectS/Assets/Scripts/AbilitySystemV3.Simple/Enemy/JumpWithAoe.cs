using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class JumpWithAoe : Ability
{
    public float meleeDamage;
    public float aoeRange;
    public float speed;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner, Animator animator)
    {
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(JumpAOECoroutine(user, animator));
    }

    IEnumerator JumpAOECoroutine(GameObject user, Animator animator)
    {
        float timer = 0;
        animator.SetTrigger(animName);
        while (timer<=animTime)
        {
            timer += Time.deltaTime;
            if (user == null) {
                abilityIsActive = false;

                yield break;
            }
            user.transform.position+=user.transform.forward * (speed * Time.deltaTime);
            yield return null;
        }
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, aoeRange);
        foreach (Collider collider in colliders)
        {
            
            if (collider.CompareTag(aim)&&collider is BoxCollider)
            {
                collider.GetComponent<HealthSystem>().TakeDamage(meleeDamage, Color.red);
            }

        }

        abilityIsActive = false;
    }
}
