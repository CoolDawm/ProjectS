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
    private bool _canActivateAb;
    //private bool _isActive=false;// for future imporvement(to not be able to use other abilities)
    private void Start()
    {
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        _currentMana = 100;
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        for (int i = 0; i < abilityList.Count; i++)
        {
            Debug.Log(abilityList[i].name);
        }
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
        for (int i = 0; i < abilityList.Count; i++)
        {
            Debug.Log(abilityList[i].name);
        }
    }

    private void Update()
    {
        _canActivateAb = true;
        if (Cursor.visible)
        {
            return;
        }

        for (int i = 0; i < abilityList.Count; i++)
        {
            if (abilityList[i])
            {
                
            }
        }
        
        if (Input.GetMouseButtonDown(0) && timers[0] <= 0)
        {
            GenerateMana(10);// нужно это убрать
            StartCoroutine(ActivateAbility(0));
            timers[0] = abilityList[0].cooldown;
        }
        
        if (Input.GetMouseButtonDown(1) && _currentMana >= abilityList[1].manaCost && timers[1] <= 0)
        {
            _currentMana -= abilityList[1].manaCost;
            StartCoroutine(ActivateAbility(1));
            timers[1] = abilityList[1].cooldown;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha1) && _currentMana >= abilityList[2].manaCost && timers[2] <= 0)
        {
            
            _currentMana -= abilityList[2].manaCost;
            StartCoroutine(ActivateAbility(2));
            timers[2] = abilityList[2].cooldown;
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2) && _currentMana >= abilityList[3].manaCost && timers[3] <= 0)
        {
            _currentMana -= abilityList[3].manaCost;
            StartCoroutine(ActivateAbility(3));
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
    IEnumerator ActivateAbility(int index)
    {
        _playerBehaviour.AttackAnim(abilityList[index].animName);
        yield return new WaitForSeconds(abilityList[index].animTime);
        abilityList[index].Activate(gameObject, _coroutineRunner);
    }
}