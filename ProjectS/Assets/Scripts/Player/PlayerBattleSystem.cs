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
    private float aoeDamage=15;
    [SerializeField]
    private float meleeDamage = 25;
    [SerializeField]
    private bool _abilityIsActive = true;
    [SerializeField]
    private float _maxMana = 100;
    private GameObject  freeLook;
    public float meleeAbilityRange;
    public float rangedAbilityProjectileLifetime;
    public float areaAbilityRadius;
    public float areaAbilityRange;
    private bool isUsingAbility;
    public float currentMana;
    public GameObject cursorObject;
    public float manaGenerationRate;
    public float meleeRange = 3f;
    public float projectileSpeed = 1500f;
    public float projectileLifeTime = 3f;
    public float areaOfEffectRadius = 10f;
    public float summonRange = 10f;
    public GameObject projectilePrefab;
    public AbilitesManager abilitiesManager;
    public UnityEvent meleeAbilityEvent;
    public UnityEvent rangeAbilityEvent;
    public UnityEvent aoeAbilityEvent;
    public UnityEvent meleeAoeAbilityEvent;
    public UnityEvent shieldAbilityEvent;
    void Start()
    {
        currentMana = _maxMana;
        freeLook = GameObject.FindGameObjectWithTag("FreeLookCamera");
        meleeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.MeleeAbility(meleeRange, meleeDamage,gameObject)));
        rangeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.RangeAbility( currentMana, projectileLifeTime,  projectileSpeed, shootingPosition)));
        aoeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.AoeAbility(currentMana,aoeDamage)));
        meleeAoeAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.MeleeAoe(meleeDamage)));
        shieldAbilityEvent.AddListener(new UnityAction(() => abilitiesManager.Shield(currentMana,gameObject)));
    }


    private void GenerateMana(float mana)
    {
        currentMana += mana;
    }


    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            meleeAbilityEvent.Invoke();
            GenerateMana(10);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            rangeAbilityEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            aoeAbilityEvent.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            meleeAoeAbilityEvent.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            shieldAbilityEvent.Invoke();
        }

    }
   
}

