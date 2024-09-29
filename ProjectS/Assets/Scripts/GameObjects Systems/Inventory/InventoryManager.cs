using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryParent; // Ссылка на родительский объект инвентаря
    private InventorySlot[] slots;

    private void Awake()
    {
        // Сохраняем ссылки на все слоты инвентаря, включая неактивные
        slots = inventoryParent.GetComponentsInChildren<InventorySlot>(true); // true чтобы включить неактивные объекты
    }

    public InventorySlot FindInventorySlot(Item itemForPickup)
    {
        //InventorySlot slotForReturn =CheckForStackableSlot(itemForPickup);
       
            foreach (InventorySlot slot in slots)
            {
                if (slot.Item == null)
                {
                    return slot;
                }
            }
            return null;
        

    }
    public InventorySlot CheckForStackableSlot(Item itemForPickup)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.item!=null&&slot.Item.itemName == itemForPickup.itemName)
            {
                return slot;
            }
        }
        return null;
    }
}
