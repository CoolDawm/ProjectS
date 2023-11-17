using System;
using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerBattleSystem : MonoBehaviour
{
    [SerializeField]
    private Transform shootingPosition;
    public float currentMana;
    public float projectileSpeed = 1500f;
    public AbilitesManager abilitiesManager;
    public Action OnmeleeAbilityEvent;
    public Action OnrangeAbilityEvent;
    public Action OnaoeAbilityEvent;
    public Action OnmeleeAoeAbilityEvent;
    public Action OnshieldAbilityEvent;
    public Action OnFrostBeamAbilityEvent;
    public Action OnSlowAuraAbilityEvent;
    public Action OnDamageAuraAbilityEvent;
    private Characteristics _characteristics;
    void Start()
    {
        _characteristics = gameObject.GetComponent<Characteristics>();
        abilitiesManager = GameObject.FindGameObjectWithTag("AbilitiesManager").GetComponent<AbilitesManager>();
        //Animations Trigger Subscription
        OnmeleeAbilityEvent += () => GetComponent<PlayerBehaviour>().AttackAnim("IsMeleeMainAttack");
        //Abilities Trigger Subscription
        OnmeleeAbilityEvent += () => abilitiesManager.MeleeAbility(_characteristics.charDic["meleeRange"], _characteristics.charDic["damage"], gameObject,80);
        OnrangeAbilityEvent += () => abilitiesManager.RangeAbility(_characteristics.charDic["projectileLife"] , projectileSpeed, shootingPosition,80);
        OnaoeAbilityEvent += () => abilitiesManager.AoeAbility(_characteristics.charDic["aoeDamage"],80);
        OnmeleeAoeAbilityEvent += () => abilitiesManager.MeleeAoe(_characteristics.charDic["aoeDamage"], gameObject,80);
        OnshieldAbilityEvent += () => abilitiesManager.Shield( gameObject,80);
        OnFrostBeamAbilityEvent += () => abilitiesManager.StartFrostBeam(shootingPosition, 1f, 10f, 25f, "Enemy",80);
        OnSlowAuraAbilityEvent += () => abilitiesManager.SlowingAura(gameObject,"Enemy",80);
        OnDamageAuraAbilityEvent += () => abilitiesManager.DamageUpAura(gameObject,"Player",80);
        Debug.Log(_characteristics.charDic.Count);
        currentMana = 100;
    }


    private void GenerateMana(float mana)
    {
        currentMana += mana;
    }


    private void Update()
    {
        //MainMelee
        if(Input.GetMouseButtonDown(0))
        {
            GenerateMana(10);
            OnmeleeAbilityEvent.Invoke();
            GenerateMana(10);
        }
        //FrostBeam
        if (Input.GetMouseButtonDown(1) && currentMana >= 30)
        {
            currentMana -= 30;
            OnFrostBeamAbilityEvent.Invoke();
        }

        //RangeProjectile
        if (Input.GetKeyDown(KeyCode.Alpha2)&&currentMana >=10)
        {
            currentMana -= 10;
            OnrangeAbilityEvent.Invoke();
        }
        //RangeAoe
        if (Input.GetKeyDown(KeyCode.Alpha3)&&currentMana >=30)
        {
            currentMana -= 30;
            OnaoeAbilityEvent.Invoke();
        }
        //MeleeAoe
        if (Input.GetKeyDown(KeyCode.Alpha4)&&currentMana >=25)
        {
            currentMana -= 25;
            OnmeleeAoeAbilityEvent.Invoke();
        }
        //Shield
        if (Input.GetKeyDown(KeyCode.Alpha5)&&currentMana >=25)
        {
            currentMana -= 25;
            OnshieldAbilityEvent.Invoke();
        }
        //Slow Aura
        if (Input.GetKeyDown(KeyCode.Alpha6)&&currentMana >=30)
        {
            currentMana -= 30;
            OnSlowAuraAbilityEvent.Invoke();
        }
        //DamageUp Aura
        if (Input.GetKeyDown(KeyCode.Alpha7)&&currentMana >=30)
        {
            currentMana -= 30;
            OnDamageAuraAbilityEvent.Invoke();
        }
    }
   
}

