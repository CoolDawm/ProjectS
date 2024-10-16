using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

[CreateAssetMenu]
public class JumpWithAoe : Ability
{
    [SerializeField]
    private float _damageBonus;
    [SerializeField]
    private float _jumpDistance;
    public float meleeDamage;
    public float aoeRange;
    public float speed;
    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        coroutineRunner.StartCoroutineFunction(JumpAOECoroutine(user));
    }

    IEnumerator JumpAOECoroutine(GameObject user)
    {
        float timer = 0;
        Animator animator = user.GetComponent<Animator>();

        float damage = 0;
        if (user.tag == "Player")
        {
            CharacterController controller = user.GetComponent<CharacterController>();
            PlayerBehaviour playerBehaviour = user.GetComponent<PlayerBehaviour>();
            playerBehaviour.ProhibitMoving();
            yield return new WaitForSeconds(0.5f);

            damage = user.GetComponent<Characteristics>().GetWeaponDamage() * _damageBonus * user.GetComponent<Characteristics>().secondCharDic["MeleeDamageMultiplyer"];
            Vector3 startPos = user.transform.position;
            Vector3 dashDir = user.transform.forward.normalized; // Нормализуйте направление 
            float remainingDistance = _jumpDistance;

            while (remainingDistance > 0)
            {
                float moveDistance = speed * Time.deltaTime;
                if (moveDistance > remainingDistance)
                {
                    moveDistance = remainingDistance;
                }

                Debug.Log("Moving");
                controller.Move(dashDir * moveDistance);
                remainingDistance -= moveDistance;
                Debug.Log(remainingDistance);
                yield return null;
            }

            yield return new WaitForSeconds(0.1f);
            Collider[] colliders = Physics.OverlapSphere(user.transform.position, aoeRange);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(aim) && collider is BoxCollider && !collider.isTrigger)
                {
                    collider.GetComponent<HealthSystem>().TakeDamage(damage, Color.red);
                }
            }
            playerBehaviour.AllowMoving();
        }
        else
        {
            animator.SetTrigger(animName);
            damage = meleeDamage;

            while (timer <= animTime)
            {
                timer += Time.deltaTime;
                if (user == null)
                {
                    abilityIsActive = false;
                    yield break;
                }
                user.transform.position += user.transform.forward * (speed * Time.deltaTime);
                yield return null;
            }

            Collider[] colliders = Physics.OverlapSphere(user.transform.position, aoeRange);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(aim) && collider is BoxCollider && !collider.isTrigger)
                {
                    collider.GetComponent<HealthSystem>().TakeDamage(damage, Color.red);
                }
            }
        }

        abilityIsActive = false;
    }

}
