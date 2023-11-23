using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AbilitiesbBar : MonoBehaviour
{
   [HideInInspector]public AbilityHolder abilityHolder;
    
    public List<Image> _spells;
    void Start()
    {
        abilityHolder = GameObject.FindWithTag("Player").GetComponent<AbilityHolder>();
        BarUpdate();
    }
    public void BarUpdate()
    {
        var abList = abilityHolder.GetAbilitiesList();
        for (int i=0; i < _spells.Count; i++)
        {
            _spells[i].sprite = abList[i].abilityImage;
        }
    }
}
