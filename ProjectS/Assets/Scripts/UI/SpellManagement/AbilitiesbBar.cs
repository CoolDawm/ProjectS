using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilitiesbBar : MonoBehaviour
{
    [HideInInspector] public AbilityHolder abilityHolder;
    private float[] _timers;
    public List<GameObject> _spells;

    void Start()
    {
        abilityHolder = GameObject.FindWithTag("Player").GetComponent<AbilityHolder>();
        _timers = abilityHolder.timers;
        //BarUpdate();
    }

    private void Update()
    {
        if (!CheckForChild())
        {
            return;
        }

        for (int i = 0; i < _timers.Length; i++)
        {
            Image img = _spells[i].transform.GetChild(0).gameObject.GetComponent<Image>();

            if (_timers[i] > 0)
            {
                img.color = Color.red;
            }
            else
            {
                _timers[i] = 0;
                img.color = Color.green;
            }
        }
    }

    public void BarUpdate()
    {
        var abList = abilityHolder.GetAbilitiesList();
        for (int i = 0; i < _spells.Count; i++)
        {
            _spells[i].GetComponent<Image>().sprite = abList[i].abilityImage;
        }
    }

    private bool CheckForChild()
    {
        bool check = false;
        for (int i = 0; i < _spells.Count; i++)
        {
            if (_spells[i].transform.childCount!=0)
            {
                check = true;
            }
            else
            {
                check = false;
            }
            
        }
        return check;
    }
}
