using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private List<Ability> abilityList;
    private Characteristics _characteristics;
    private CoroutineRunner _coroutineRunner;
    private float _currentMana;
    private void Start()
    {
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        _currentMana = 100;
    }
    private void GenerateMana(float mana)
    {
        _currentMana += mana;
    }
    public List<Ability> GetAbilitiesList()
    {
        return abilityList;
    }
    public void ChangeAbility(Ability ability,int place)
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
        if(Input.GetMouseButtonDown(0))
        {
            GenerateMana(10);
            abilityList[0].Activate(gameObject,_coroutineRunner);
        }
        //FrostBeam
        if (Input.GetMouseButtonDown(1)&& _currentMana >= abilityList[1].manaCost )
        {
            _currentMana-=abilityList[1].manaCost;
            abilityList[1].Activate(gameObject,_coroutineRunner);
        }

        //RangeProjectile
        if (Input.GetKeyDown(KeyCode.Alpha1)&& _currentMana >= abilityList[2].manaCost)
        {
            _currentMana-=abilityList[2].manaCost; 
            abilityList[2].Activate(gameObject,_coroutineRunner);
        }
        //RangeAoe
        if (Input.GetKeyDown(KeyCode.Alpha2)&& _currentMana >= abilityList[3].manaCost)
        {
            _currentMana-=abilityList[3].manaCost;
            abilityList[3].Activate(gameObject,_coroutineRunner);
        }
    }
}