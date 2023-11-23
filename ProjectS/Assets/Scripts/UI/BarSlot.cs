using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarSlot : MonoBehaviour,IDropHandler
{
    public int slotNumber;
    private AbilitiesbBar _abilitiesbBar;
    void Start()
    {
        _abilitiesbBar = GetComponentInParent<AbilitiesbBar>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragableItem dragableItem = dropped.GetComponent<DragableItem>();
        dragableItem.parentAfterDrag = transform;
        Ability ability = dragableItem.ability;
        Debug.Log(ability.name);
        _abilitiesbBar.abilityHolder.ChangeAbility(ability,slotNumber);
    }
}
