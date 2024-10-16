using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class VampiricMelee : Ability
{
    [SerializeField]
    private float _damageBonus;
    [SerializeField]
    private float _lifeStealPercentage = 0.3f; 
    private int _comboCount = 0;
    private float _comboTimer = 2.0f;
    private Coroutine _comboCoroutine;

    public override void Activate(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;
        if (_comboCoroutine != null)
        {
            coroutineRunner.StopCoroutine(_comboCoroutine);
        }

        _comboCoroutine = coroutineRunner.StartCoroutine(HandleCombo(user));
        abilityIsActive = false;
    }

    private IEnumerator HandleCombo(GameObject user)
    {
        if (_comboCount < 4)
        {
            _comboCount++;
        }
        else
        {
            _comboCount = 0;
        }
        Debug.Log(user.GetComponent<Characteristics>().GetWeaponDamage());
        float damage = user.GetComponent<Characteristics>().GetWeaponDamage() * _damageBonus * (1 + (_comboCount - 1) * 0.1f) * user.GetComponent<Characteristics>().secondCharDic["MeleeDamageMultiplyer"];

        Collider[] colliders = Physics.OverlapSphere(user.transform.position, range);
        foreach (Collider collider in colliders)
        {
        
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            if (Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {
                if (collider.transform.root.CompareTag(aim) && collider is BoxCollider && !collider.isTrigger)
                {
                    collider.transform.root.GetComponent<HealthSystem>().TakeDamage(damage, Color.white,_elementName);
                    user.GetComponent<AbilitiesHolder>().GenerateMana(damage / 5);
                    user.GetComponent<HealthSystem>().IncreaseCurrentHealth(damage * _lifeStealPercentage);
                }
            }
        }

        yield return new WaitForSeconds(_comboTimer);

        _comboCount = 0;
        _comboCoroutine = null;
    }
}
