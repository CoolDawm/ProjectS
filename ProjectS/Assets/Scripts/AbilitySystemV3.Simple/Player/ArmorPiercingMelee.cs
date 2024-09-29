using System.Collections;
using UnityEngine;

[CreateAssetMenu]
public class ArmorPiercingMelee : Ability
{
    [SerializeField]
    private float _damageBonus;
    [SerializeField]
    private float _armorPiercingFactor = 0.5f; 
    [SerializeField]
    private float _strongAttackMultiplier = 3.0f;
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
        if (_comboCount < 3)
        {
            _comboCount++;
        }
        else
        {
            _comboCount = 0;
        }
        float damage = user.GetComponent<Characteristics>().GetWeaponDamage() * _damageBonus * (1 + (_comboCount - 1) * 0.1f) * user.GetComponent<Characteristics>().secondCharDic["MeleeDamageMultiplyer"];

        Collider[] colliders = Physics.OverlapSphere(user.transform.position, range);
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            if (Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {
                if (collider.transform.root.CompareTag(aim) && collider is BoxCollider)
                {
                    HealthSystem health = collider.transform.root.GetComponent<HealthSystem>();
                    float effectiveDamage = damage + (1 + health.currentShield * _armorPiercingFactor);
                    health.TakeDamage(effectiveDamage, Color.red);
                    user.GetComponent<AbilityHolder>().GenerateMana(damage / 5);
                }
            }
        }

        yield return new WaitForSeconds(_comboTimer);

        _comboCount = 0;
        _comboCoroutine = null;
    }
    public override IEnumerator ActivateStrongAttack(GameObject user, CoroutineRunner coroutineRunner)
    {
        abilityIsActive = true;

        float damage = user.GetComponent<Characteristics>().GetWeaponDamage()  * _strongAttackMultiplier * user.GetComponent<Characteristics>().secondCharDic["MeleeDamageMultiplyer"];
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, range);

        foreach (Collider collider in colliders)
        {
            
            if (collider.transform.root.CompareTag(aim) && collider is BoxCollider)
            {
                HealthSystem health = collider.transform.root.GetComponent<HealthSystem>();
                float effectiveDamage = damage+(1 + health.currentShield * 1);
                health.TakeDamage(effectiveDamage, Color.red);
                user.GetComponent<AbilityHolder>().GenerateMana(damage / 5);
            }
        }

        _comboCount = 0; 
        yield return null;
        abilityIsActive = false;
    }

}
