using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private List<Ability> abilityList;
    [HideInInspector]public float[] timers= new float[4]{0,0,0,0};
    private Characteristics _characteristics;
    private CoroutineRunner _coroutineRunner;
    private float _currentMana;
    private PlayerBehaviour _playerBehaviour;
   
    //private bool _isActive=false;// for future imporvement(to not be able to use other abilities)
    private void Start()
    {
        _coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
        _currentMana = 100;
        _playerBehaviour=GetComponent<PlayerBehaviour>();
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
        if(Input.GetMouseButtonDown(0)&&Time.time > timers[0] + abilityList[0].cooldown)
        {
            GenerateMana(10);
            abilityList[0].Activate(gameObject,_coroutineRunner);
           _playerBehaviour.AttackAnim("IsMeleeMainAttack");
           timers[0] = Time.time;
        }
        //FrostBeam
        if (Input.GetMouseButtonDown(1)&& _currentMana >= abilityList[1].manaCost && Time.time > timers[1] + abilityList[1].cooldown )
        {
            _currentMana-=abilityList[1].manaCost;
            abilityList[1].Activate(gameObject,_coroutineRunner);
            timers[1] = Time.time;
        }

        //RangeProjectile
        if (Input.GetKeyDown(KeyCode.Alpha1)&& _currentMana >= abilityList[2].manaCost && Time.time > timers[2] + abilityList[2].cooldown)
        {
            _currentMana-=abilityList[2].manaCost; 
            abilityList[2].Activate(gameObject,_coroutineRunner);
            timers[2] = Time.time;
        }
        //RangeAoe
        if (Input.GetKeyDown(KeyCode.Alpha2)&& _currentMana >= abilityList[3].manaCost && Time.time > timers[3] + abilityList[3].cooldown)
        {
            _currentMana-=abilityList[3].manaCost;
            abilityList[3].Activate(gameObject,_coroutineRunner);
            timers[3] = Time.time;
        }
    }
}