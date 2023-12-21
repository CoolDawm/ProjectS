using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BarSlot : MonoBehaviour,IDropHandler
{
    public int slotNumber;
    public GameObject spell;
    private AbilitiesbBar _abilitiesbBar;
    private bool excTriggered;
    void Start()
    {
        _abilitiesbBar = GetComponentInParent<AbilitiesbBar>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DragableItem dragableItem = dropped.GetComponent<DragableItem>();
        if (dragableItem.inSpellBook)
        {
            Destroy(spell);
            spell = dropped;
            dragableItem.parentAfterDrag = transform;
            Ability ability = dragableItem.ability;
            _abilitiesbBar.abilityHolder.ChangeAbility(ability,slotNumber);
            _abilitiesbBar.BarUpdate();
        }
        else
        {
            spell.transform.SetParent(dragableItem.startPosition);
            GameObject tmpSlot=spell.transform.parent.gameObject;
            tmpSlot.GetComponent<BarSlot>()._abilitiesbBar.abilityHolder.ChangeAbility(spell.GetComponent<DragableItem>().ability,tmpSlot.GetComponent<BarSlot>().slotNumber);
            spell = dropped;
            dragableItem.parentAfterDrag = transform;
            Ability ability = dragableItem.ability;
            _abilitiesbBar.abilityHolder.ChangeAbility(ability,slotNumber);
            _abilitiesbBar.BarUpdate();
        }
    }
}
