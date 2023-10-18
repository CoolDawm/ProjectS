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
    [SerializeField]
    private GameObject  freeLook;
    private Characteristics _characteristics;
    private bool isUsingAbility;
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
    void Start()
    {
        freeLook = GameObject.FindGameObjectWithTag("FreeLookCamera");
        _characteristics = gameObject.GetComponent<Characteristics>();
        currentMana = _characteristics.charDic["maxmana"];
        OnmeleeAbilityEvent += () => abilitiesManager.MeleeAbility(_characteristics.charDic["meleeRange"], _characteristics.charDic["damage"], gameObject);
        OnrangeAbilityEvent += () => abilitiesManager.RangeAbility(currentMana,_characteristics.charDic["projectileLife"] , projectileSpeed, shootingPosition);
        OnaoeAbilityEvent += () => abilitiesManager.AoeAbility(currentMana, _characteristics.charDic["aoeDamage"]);
        OnmeleeAoeAbilityEvent += () => abilitiesManager.MeleeAoe(_characteristics.charDic["aoeDamage"], gameObject);
        OnshieldAbilityEvent += () => abilitiesManager.Shield(currentMana, gameObject);
        OnFrostBeamAbilityEvent += () => abilitiesManager.StartFrostBeam(shootingPosition, 1f, 10f, 25f, "Enemy");
        OnSlowAuraAbilityEvent += () => abilitiesManager.SlowingAura(gameObject,"Enemy");
        OnDamageAuraAbilityEvent += () => abilitiesManager.DamageUpAura(gameObject,"Player");
    }


    private void GenerateMana(float mana)
    {
        currentMana += mana;
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            OnmeleeAbilityEvent.Invoke();
            GenerateMana(10);
        }
        if(Input.GetMouseButtonDown(1))
        {
            OnFrostBeamAbilityEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            OnrangeAbilityEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            OnaoeAbilityEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            OnmeleeAoeAbilityEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            OnshieldAbilityEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            OnSlowAuraAbilityEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            OnDamageAuraAbilityEvent.Invoke();
        }
    }
   
}

