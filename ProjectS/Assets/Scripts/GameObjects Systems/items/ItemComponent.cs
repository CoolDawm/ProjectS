using System;
using UnityEngine;

public class ItemComponent : MonoBehaviour
{
    private Item item;
    public KeyCode pickUpKey = KeyCode.F;
    private bool _canBePickedUp = false;
    private InventoryManager _inventoryManager;
    private Light _pickupMarker;
    public bool isRewardToChoose;
    public Action onChoose;
    private void Start()
    {
        _inventoryManager=FindObjectOfType<InventoryManager>();
        _pickupMarker = GetComponentInChildren<Light>();

    }
    private void Update()
    {
        if (_canBePickedUp && Input.GetKeyDown(pickUpKey))
        {
            TryPickUpItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canBePickedUp = true;
            _pickupMarker.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _canBePickedUp = false;
            _pickupMarker.enabled = false;
        }
    }

    public void SetItem(Item newItem)
    {
        item = newItem;
    }
    public void DestroyNotChosenItem()
    {
        Destroy(gameObject);
    }

    private void TryPickUpItem()
    {

        InventorySlot freeSlot = _inventoryManager.CheckForStackableSlot(item);
        if (freeSlot == null)
        {
            freeSlot=_inventoryManager.FindInventorySlot(item);
            if (freeSlot != null)
            {
                freeSlot.AddItem(item);
                if (isRewardToChoose)
                {
                    onChoose?.Invoke();
                }
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Нет свободных ячеек в инвентаре");
            }
        }
        else
        {
            freeSlot.IncreaseStackCount();
            Destroy(gameObject);

        }

    }
}
