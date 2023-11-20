using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpellBookShow : MonoBehaviour
{
    [SerializeField]private SpellBook _spellBook;
    [SerializeField] private List<GameObject> abPanels;
    private AbilitiesbBar _abilitiesbBar;
    void Start()
    {
        AbilitiesShow();
        _abilitiesbBar = GetComponentInParent<AbilitiesbBar>();
    }

    private void OnEnable()
    {
        AbilitiesShow();
    }

    public void AbilitiesShow()
    {
        for (int i = 0; i < abPanels.Count; i++)
        {
            abPanels[i].transform.Find("Image").GetComponent<Image>().sprite = _spellBook.abilities[i].abilityImage;
            abPanels[i].transform.Find("Description").GetComponent<Text>().text = _spellBook.abilities[i].description;
            abPanels[i].transform.Find("Cost").GetComponent<Text>().text = _spellBook.abilities[i].manaCost.ToString();
        }
    }
    
}
