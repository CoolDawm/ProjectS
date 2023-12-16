using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private List<Ability> abilityList;
    [HideInInspector] public float[] timers = new float[4] { 0, 0, 0, 0 };
    private Characteristics _characteristics;
    private CoroutineRunner _coroutineRunner;
    private float _currentMana;
    private PlayerBehaviour _playerBehaviour;

    //private bool _isActive=false;// for future imporvement(to not be able to use other abilities)
    private void Start()
    {
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        _currentMana = 100;
        _playerBehaviour = GetComponent<PlayerBehaviour>();
    }

    private void GenerateMana(float mana)
    {
        _currentMana += mana;
    }

    public List<Ability> GetAbilitiesList()
    {
        return abilityList;
    }

    public void ChangeAbility(Ability ability, int place)
    {
        abilityList[place] = ability;
    }

    private void Update()
    {
        if (Cursor.visible)
        {
            return;
        }

        //MainMelee
        if (Input.GetMouseButtonDown(0) && timers[0] <= 0)
        {
            _playerBehaviour.AttackAnim(abilityList[0].animName);
            GenerateMana(10);
            abilityList[0].Activate(gameObject, _coroutineRunner);
            timers[0] = abilityList[0].cooldown;
        }

        //FrostBeam
        if (Input.GetMouseButtonDown(1) && _currentMana >= abilityList[1].manaCost && timers[1] <= 0)
        {
            _currentMana -= abilityList[1].manaCost;
            _playerBehaviour.AttackAnim(abilityList[1].animName);
            abilityList[1].Activate(gameObject, _coroutineRunner);
            timers[1] = abilityList[1].cooldown;
        }

        //RangeProjectile
        if (Input.GetKeyDown(KeyCode.Alpha1) && _currentMana >= abilityList[2].manaCost && timers[2] <= 0)
        {
            _playerBehaviour.AttackAnim(abilityList[2].animName);
            _currentMana -= abilityList[2].manaCost;
            abilityList[2].Activate(gameObject, _coroutineRunner);
            timers[2] = abilityList[2].cooldown;
        }

        //RangeAoe
        if (Input.GetKeyDown(KeyCode.Alpha2) && _currentMana >= abilityList[3].manaCost && timers[3] <= 0)
        {
            _playerBehaviour.AttackAnim(abilityList[3].animName);
            _currentMana -= abilityList[3].manaCost;
            abilityList[3].Activate(gameObject, _coroutineRunner);
            timers[3] = abilityList[3].cooldown;
        }

        for (int i = 0; i < timers.Length; i++)
        {
            if (timers[i] > 0)
            {
                timers[i] -= Time.deltaTime;
            }
            else
            {
                timers[i] = 0;
            }
        }
    }
}