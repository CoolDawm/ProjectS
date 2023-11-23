using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookShow : MonoBehaviour
{
    [SerializeField]private SpellBook _spellBook;
    [SerializeField] private List<GameObject> abPanels;
    
    void Start()
    {
        AbilitiesShow();
        
    }

    private void OnEnable()
    {
        AbilitiesShow();
    }

    public void AbilitiesShow()
    {
        for (int i = 0; i < abPanels.Count; i++)
        {
            abPanels[i].transform.Find("SpellSlot").GetComponentInChildren <Image>().sprite = _spellBook.abilities[i].abilityImage;
            abPanels[i].transform.Find("Description").GetComponent<Text>().text = _spellBook.abilities[i].description;
            abPanels[i].transform.Find("Cost").GetComponent<Text>().text = _spellBook.abilities[i].manaCost.ToString();
            abPanels[i].GetComponentInChildren<DragableItem>().ability = _spellBook.abilities[i];
        }
    }
    
}
