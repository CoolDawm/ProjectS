using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    [SerializeField] private List<Ability> abilityList;
    private CoroutineRunner coroutineRunner;

    private void Start()
    {
        coroutineRunner = GameObject.FindGameObjectWithTag("CoroutineRunner").GetComponent<CoroutineRunner>();
    }

    public List<Ability> GetAbilitiesList()
    {
        return abilityList;
    }
    public void ChangeAbility(Ability ability,int place)
    {
        Debug.Log(abilityList[place].name);
        abilityList[place] = ability;
        Debug.Log(abilityList[place].name);
    }
    private void Update()
    {
        //MainMelee
        if(Input.GetMouseButtonDown(0))
        {
            abilityList[0].Activate(gameObject,coroutineRunner);
        }
        //FrostBeam
        if (Input.GetMouseButtonDown(1) )
        {
            abilityList[1].Activate(gameObject,coroutineRunner);
        }

        //RangeProjectile
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            abilityList[2].Activate(gameObject,coroutineRunner);
        }
        //RangeAoe
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            abilityList[3].Activate(gameObject,coroutineRunner);
        }
    }
}
