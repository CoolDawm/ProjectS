using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class SimpleMeleeWithAOE : Ability
{
    [SerializeField]
    private float _damageBonus;
    [SerializeField]
    private float _strongAttackMultiplier = 3.0f;
    [SerializeField]
    private int _ticksAmount = 0;
    [SerializeField]
    private float _timeBetweenTicks = 0;
    [SerializeField]
    private GameObject _aoePrefab;
    public float areaOfEffectRadius = 5f;
    private GameObject _aoeEffect;
   
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
        if (_comboCount < 5)
        {
            _comboCount++;
        }
        else
        {
            _comboCount = 0;
        }
        Debug.Log("Combo " + _comboCount);
        Debug.Log(user.GetComponent<Characteristics>().GetWeaponDamage());

        float damage = user.GetComponent<Characteristics>().GetWeaponDamage() * _damageBonus * (1 + (_comboCount - 1) * 0.1f) * user.GetComponent<Characteristics>().secondCharDic["MeleeDamageMultiplyer"];
        Collider[] colliders = Physics.OverlapSphere(user.transform.position, range);
        foreach (Collider collider in colliders)
        {
            Vector3 directionToTarget = collider.transform.position - user.transform.position;
            if (Vector3.Dot(user.transform.forward, directionToTarget) > 0)
            {
                if (collider.transform.root.CompareTag(aim) && collider is BoxCollider)
                {
                    collider.transform.root.GetComponent<HealthSystem>().TakeDamage(damage, Color.white);
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
        float damage = user.GetComponent<Characteristics>().GetWeaponDamage() * _strongAttackMultiplier * user.GetComponent<Characteristics>().secondCharDic["MeleeDamageMultiplyer"];

        int ticksCount = 0;
        Collider[] colliders;
        _aoeEffect = Instantiate(_aoePrefab, user.transform.position, Quaternion.identity, user.transform);
        Debug.DrawRay(user.transform.position, Vector3.up, Color.red, 5f);
        while (abilityIsActive)
        {
            colliders = Physics.OverlapSphere(user.transform.position, areaOfEffectRadius);
            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag(aim))
                {
                    collider.transform.root.GetComponent<HealthSystem>().TakeDamage(damage, Color.white);
                    user.GetComponent<AbilityHolder>().GenerateMana(damage / 5);
                }
            }
            yield return new WaitForSeconds(_timeBetweenTicks);
            ticksCount++;
            if (ticksCount >= _ticksAmount)
            {
                abilityIsActive = false;
            }
            yield return null;
        }
        Destroy(_aoeEffect);
        _comboCount = 0;
        Debug.Log("Combo to 0");
        yield return null;
        abilityIsActive = false;
    }
  
}
