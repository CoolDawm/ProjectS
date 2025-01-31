using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilitiesbBar : MonoBehaviour
{
    [HideInInspector] public AbilitiesHolder abilityHolder;
    public List<GameObject> _spells;
    private float[] _timers;
    private bool spellExist;
    void Start()
    {
        GameObject spellSlot = Resources.Load<GameObject>("UI/Spell");
        abilityHolder = GameObject.FindWithTag("Player").GetComponent<AbilitiesHolder>();
        _timers = abilityHolder.timers;
        List<Ability> abilityList=abilityHolder.GetAbilitiesList();
        for (int i = 0; i < abilityList.Count; i++)
        {
            GameObject spell = Instantiate(spellSlot, _spells[i].transform);
            _spells[i].GetComponent<BarSlot>().spell=spell;
            spell.GetComponent<DragableItem>().ability = abilityList[i];
        }
        BarUpdate();
    }

    private void Update()
    {
        if (!CheckForChild())
        {
            return;
        }

        for (int i = 0; i < _timers.Length; i++)
        {
            spellExist = true;
            try
            {
                Image img = _spells[i].transform.GetChild(0).gameObject.GetComponent<Image>();
            }
            catch (Exception e)
            {
                spellExist = false;
            }

            if (spellExist)
            {
                
                Image img = _spells[i].transform.GetChild(0).gameObject.GetComponent<Image>();
                if (_timers[i] > 0)
                {
                    img.fillAmount =Mathf.Lerp(-(_timers[i])+1,1,Time.deltaTime);
                }
                else
                {
                    _timers[i] = 0;
                }
            }
           
        }
    }

    public void BarUpdate()
    {
        var abList = abilityHolder.GetAbilitiesList();
        for (int i = 0; i < _spells.Count; i++)
        {
            _spells[i].GetComponentInChildren<Image>().sprite = abList[i].abilityImage;
            _spells[i].transform.GetChild(0).gameObject.GetComponentInChildren<Image>().type = Image.Type.Filled;
            _spells[i].transform.GetChild(0).gameObject.GetComponentInChildren<Image>().sprite = abList[i].abilityImage;
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
