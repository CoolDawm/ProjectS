using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesbBar : MonoBehaviour
{
    private AbilityHolder _abilityHolder;
    
    public List<Button> _buttons;
    void Start()
    {
        _abilityHolder = GetComponentInParent<AbilityHolder>();
        BarUpdate();
    }
    public void BarUpdate()
    {
        var abList = _abilityHolder.GetAbilitiesList();
        for (int i=0; i < _buttons.Count; i++)
        {
            _buttons[i].image.sprite = abList[i].abilityImage;
        }
    }
}
