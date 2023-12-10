using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarSlot : MonoBehaviour,IDropHandler
{
    public int slotNumber;
    private AbilitiesbBar _abilitiesbBar;
    private bool isEmpty = false;
    private GameObject spell;
    private bool excTriggered;
    void Start()
    {
        _abilitiesbBar = GetComponentInParent<AbilitiesbBar>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragableItem dragableItem = dropped.GetComponent<DragableItem>();
        if (spell != null)
        {
            spell.transform.SetParent(dragableItem.startPosition);
            spell = null;
            Debug.Log(spell);
        }
        if (spell == null)
        {
            spell = dropped;
            dragableItem.parentAfterDrag = transform;
            Ability ability = dragableItem.ability;
            _abilitiesbBar.abilityHolder.ChangeAbility(ability,slotNumber);
            _abilitiesbBar.BarUpdate();
            isEmpty = false;
            Debug.Log(gameObject.transform.childCount);
        }
    }
}
