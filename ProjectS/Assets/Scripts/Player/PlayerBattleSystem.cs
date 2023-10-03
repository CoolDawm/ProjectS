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
    public float aoeDamage=15;
    public float meleeDamage = 25;
    [SerializeField]
    private bool _abilityIsActive = true;
    public float _maxMana = 100;
    private GameObject  freeLook;
    private Characteristics _characteristics;
    private bool isUsingAbility;
    public float currentMana;
    public float manaGenerationRate;
    public float meleeRange = 3f;
    public float projectileSpeed = 1500f;
    public float projectileLifeTime = 3f;
    public AbilitesManager abilitiesManager;
    public Action OnmeleeAbilityEvent;
    public Action OnrangeAbilityEvent;
    public Action OnaoeAbilityEvent;
    public Action OnmeleeAoeAbilityEvent;
    public Action OnshieldAbilityEvent;
    public Action OnFrostBeamAbilityEvent;
    void Start()
    {
        currentMana = _maxMana;
        freeLook = GameObject.FindGameObjectWithTag("FreeLookCamera");
        _characteristics = gameObject.GetComponent<Characteristics>();
        OnmeleeAbilityEvent += () => abilitiesManager.MeleeAbility(meleeRange, meleeDamage, gameObject);
        OnrangeAbilityEvent += () => abilitiesManager.RangeAbility(currentMana, projectileLifeTime, projectileSpeed, shootingPosition);
        OnaoeAbilityEvent += () => abilitiesManager.AoeAbility(currentMana, aoeDamage);
        OnmeleeAoeAbilityEvent += () => abilitiesManager.MeleeAoe(meleeDamage, gameObject);
        OnshieldAbilityEvent += () => abilitiesManager.Shield(currentMana, gameObject);
        OnFrostBeamAbilityEvent += () => abilitiesManager.StartFrostBeam(shootingPosition, 1f, 10f, 25f, "Enemy");
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

    }
   
}

