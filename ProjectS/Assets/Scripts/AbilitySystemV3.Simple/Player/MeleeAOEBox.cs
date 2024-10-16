using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class MeleeAOEBox : Ability
{
    [SerializeField]
    private float _damageBonus;
    private Coroutine _attackCoroutine;
    private float attackAreaSize = 3f; 

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        if (_attackCoroutine != null)
        {
            coroutineRunner.StopCoroutine(_attackCoroutine);
        }

        _attackCoroutine = coroutineRunner.StartCoroutine(HandleAttack(user,coroutineRunner)); 
    }

    private IEnumerator HandleAttack(GameObject user,CoroutineRunner corRunner) 
    {
        
        float damage = user.GetComponent<Characteristics>().GetWeaponDamage() * _damageBonus * user.GetComponent<Characteristics>().secondCharDic["MeleeDamageMultiplyer"];
        Vector3 center = user.transform.position + user.transform.forward*4;
        Collider[] colliders = Physics.OverlapBox(center, new Vector3(attackAreaSize, 1, attackAreaSize), Quaternion.identity);
        List<GameObject> targetsForStun = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            if (collider.transform.root.CompareTag(aim) && collider is BoxCollider && !collider.isTrigger)
            {
                collider.transform.root.GetComponent<HealthSystem>().TakeDamage(damage, Color.white,_elementName);
                corRunner.StartCoroutineFunction(StunForTime(collider.gameObject, 2));

            }
        }

        _attackCoroutine = null;
        yield return null; 
        abilityIsActive = false; 
    }
    private IEnumerator StunForTime(GameObject target, float time)
    {

        if (target.GetComponent<EnemyBehaviour>())
        {
            target.GetComponent<EnemyBehaviour>().Stun();

            yield return new WaitForSeconds(time);

            target.GetComponent<EnemyBehaviour>().UnStun();
        }



        yield return null;
    }

}
