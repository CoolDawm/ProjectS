using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class InventorySlot : MonoBehaviour, ISlot, IDropHandler, IPointerClickHandler
{
    public Image icon;
    //public Button removeButton;
    public Item item;
    public Item Item => item;
    private int _itemStackCount = 0;
    public Action onItemInfoUpdate;
    private void Start()
    {
        if (item != null)
        {
            AddItem(item);
        }
    }

    public void AddItem(Item newItem)
    {
        Debug.Log(newItem.icon);
        item = newItem;
        Debug.Log(icon);
        icon.sprite = item.icon;
        if (item.isStackable)
        {
            _itemStackCount++;
        }
      //  icon.enabled = true;
       // removeButton.interactable = true;
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        _itemStackCount = 0;
       // icon.enabled = false;
    }

    public void OnRemoveButton()
    {
        Inventory.instance.Remove(item);
        ClearSlot();
    }

    public void UseItem()
    {
        if (item != null)
        {
            // Add functionality to use the item here
        }
    }

    public bool CanAcceptItem(Item item)
    {
        Debug.Log("In"+gameObject.name);
        return true;
    }

    public void OnDrop(PointerEventData eventData)
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
        if (item != null && !otherSlot.CanAcceptItem(item)) return;

        if (item != null && otherSlot.CanAcceptItem(item))
        {
            otherSlot.AddItem(item);
            if (item.isStackable)
            {
                otherSlot.SetStackCount(_itemStackCount);
                _itemStackCount = 0;
            }
        }
        else
        {
            otherSlot.ClearSlot();
        }

        if (tempItem != null)
        {
            AddItem(tempItem);
            if (tempItem.isStackable)
            {
                otherSlot.GetStackCount();
            }
        }
        else
        {
            ClearSlot();
        }
    }
    public void SetStackCount(int count)
    {
        _itemStackCount = count;
        onItemInfoUpdate?.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (item!=null&& item.GetExpFromItem()>0)
        {
            PlayerLevelSystem _playerLevelSystem = FindObjectOfType<PlayerLevelSystem>();//_target.GetComponent<PlayerLevelSystem>();
            _playerLevelSystem.AddExp(item.GetExpFromItem());
            if (_itemStackCount-- <= 0)
            {
                onItemInfoUpdate?.Invoke();

                Destroy(item);
                ClearSlot();
            }
            else
            {

                _itemStackCount--;
                onItemInfoUpdate?.Invoke();

            }

        }
    }
    public void IncreaseStackCount()
    {
        _itemStackCount++;
        onItemInfoUpdate?.Invoke();

    }
    public int GetStackCount()
    {
        return _itemStackCount;
    }
}
