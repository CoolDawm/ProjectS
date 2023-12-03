using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilitiesbBar : MonoBehaviour
{
   [HideInInspector]public AbilityHolder abilityHolder;
   private float[] _timers;
   private List<float> _cooldowns=new List<float>();
    public List<GameObject> _spells;
    void Start()
    {
        abilityHolder = GameObject.FindWithTag("Player").GetComponent<AbilityHolder>();
        _timers = abilityHolder.timers;
        BarUpdate();
    }

    private void Update()
    {
        if(Time.time>_timers[0] +  _cooldowns[0])
        {
            _spells[0].GetComponentInChildren<Image>().color=Color.red;
        } 
        if (Time.time > _timers[1] + _cooldowns[1])
        {
            _spells[1].GetComponentInChildren<Image>().color=Color.red;
        }

        if (Time.time > _timers[2] + _cooldowns[2])
        {
            _spells[2].GetComponentInChildren<Image>().color=Color.red;
        }

        if (Time.time > _timers[3] + _cooldowns[3])
        {
            _spells[3].GetComponentInChildren<Image>().color=Color.red;
        }
    }

    public void BarUpdate()
    {
        var abList = abilityHolder.GetAbilitiesList();
        for (int i=0; i < _spells.Count; i++)
        {
            _spells[i].GetComponentInChildren<Image>().sprite = abList[i].abilityImage;
            _cooldowns.Add(abList[i].cooldown);
        }
    }
}
