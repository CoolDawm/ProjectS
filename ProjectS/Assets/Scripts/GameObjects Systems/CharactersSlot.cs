using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterSlot : MonoBehaviour, ISlot, IDropHandler
{
    public Image icon;
    public Item item;

    public Item Item => item;

    private void Start()
    {
        if (item != null)
        {
            AddItem(item);
        }
    }

    public virtual void AddItem(Item newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
       // icon.enabled = true;
    }

    public virtual void ClearSlot()
    {
        item = null;
        icon.sprite = null;
       // icon.enabled = false;
    }

    public virtual bool CanAcceptItem(Item item)
    {
        Debug.Log("1");
        return false; 
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        ItemDragHandler dragHandler = eventData.pointerDrag.GetComponent<ItemDragHandler>();
        if (dragHandler != null)
        {
            SwapItems(dragHandler.parentSlot);
        }
    }

    void SwapItems(ISlot otherSlot)
    {
        Item tempItem = otherSlot.Item;
        Debug.Log(tempItem);
        if (tempItem != null && !CanAcceptItem(tempItem)) return;

        if (item != null)
        {
            otherSlot.AddItem(item);
        }
        else
        {
            otherSlot.ClearSlot();
        }
        Debug.Log(tempItem);

        if (tempItem != null && CanAcceptItem(tempItem))
        {
            Debug.Log("Swapped");
            AddItem(tempItem);
        }
        else
        {
            ClearSlot();
        }
    }

    public void SetStackCount(int count)
    {
        throw new System.NotImplementedException();
    }

    public int GetStackCount()
    {
        throw new System.NotImplementedException();
    }
}
