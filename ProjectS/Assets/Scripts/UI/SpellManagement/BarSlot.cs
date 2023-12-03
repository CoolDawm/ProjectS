using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarSlot : MonoBehaviour,IDropHandler
{
    public int slotNumber;
    private AbilitiesbBar _abilitiesbBar;
    private bool isEmpty = false;
    void Start()
    {
        _abilitiesbBar = GetComponentInParent<AbilitiesbBar>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        Transform spell = gameObject.transform.Find("Spell");
        if (spell != null)
        {
            Destroy(spell.gameObject);
            isEmpty = true;
        }
        if (spell == null)
        {
            //probably didnt  seethe object NEED TO FIX
            isEmpty = true;
        }
        if (isEmpty)
        {
            GameObject dropped = eventData.pointerDrag;
            DragableItem dragableItem = dropped.GetComponent<DragableItem>();
            dragableItem.parentAfterDrag = transform;
            Ability ability = dragableItem.ability;
            _abilitiesbBar.abilityHolder.ChangeAbility(ability,slotNumber);
            _abilitiesbBar.BarUpdate();
            isEmpty = false;
            Debug.Log(gameObject.transform.childCount);
        }
    }
}
