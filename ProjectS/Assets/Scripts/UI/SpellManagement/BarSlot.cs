using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarSlot : MonoBehaviour,IDropHandler
{
    public int slotNumber;
    private AbilitiesbBar _abilitiesbBar;
    private bool isEmpty = true;
    void Start()
    {
        _abilitiesbBar = GetComponentInParent<AbilitiesbBar>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        if (isEmpty)
        {
            GameObject dropped = eventData.pointerDrag;
            DragableItem dragableItem = dropped.GetComponent<DragableItem>();
            dragableItem.parentAfterDrag = transform;
            Ability ability = dragableItem.ability;
            _abilitiesbBar.abilityHolder.ChangeAbility(ability,slotNumber);
            isEmpty = false;
        }
        else
        {
            Destroy(gameObject.transform.Find("Spell").gameObject);
            GameObject dropped = eventData.pointerDrag;
            DragableItem dragableItem = dropped.GetComponent<DragableItem>();
            dragableItem.parentAfterDrag = transform;
            Ability ability = dragableItem.ability;
            _abilitiesbBar.abilityHolder.ChangeAbility(ability,slotNumber);
        }
       
    }
}
