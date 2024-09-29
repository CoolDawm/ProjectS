using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPrefab; 
    private GameObject tooltipInstance;
    private InventorySlot inventorySlot;

    private void Awake()
    {
        inventorySlot = GetComponent<InventorySlot>();
        inventorySlot.onItemInfoUpdate += UpdateTootipInformation;
    }
    private void OnDisable()
    {
        Destroy(tooltipInstance);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (inventorySlot.item != null && tooltipPrefab != null)
        {
            tooltipInstance = Instantiate(tooltipPrefab, transform.root);
            tooltipInstance.transform.SetAsLastSibling();
            CanvasGroup canvasGroup = tooltipInstance.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = tooltipInstance.AddComponent<CanvasGroup>();
            }
            canvasGroup.blocksRaycasts = false;
            TextMeshProUGUI tooltipText = tooltipInstance.GetComponentInChildren<TextMeshProUGUI>();
            if (tooltipText != null)
            {
                string tooltipContent = inventorySlot.item.itemName;
                if (inventorySlot.item.isStackable)
                {
                    tooltipContent += $"\nCount: {inventorySlot.GetStackCount()}\n";

                }
                if (inventorySlot.item is Weapon weapon)
                {
                    tooltipContent += $"\nDamage: {weapon.damage}\n";
                    tooltipContent += $"\nRange: {weapon.range}\n";

                    tooltipContent += $"\nElement: {weapon._weaponElement}\n";

                    var bonuses = weapon.CharacteristicsBonuses();
                    if (bonuses != null && bonuses.Count > 0)
                    {
                        tooltipContent += "Bonuses:ò \n";
                        foreach (var bonus in bonuses)
                        {
                            tooltipContent += $"- {bonus}";
                        }
                    }
                }
                else if (inventorySlot.item is Armor armor)
                {
                    tooltipContent += $"\nDefense: {armor.defense}";
                }

                tooltipText.text = tooltipContent;
            }

            RectTransform tooltipRectTransform = tooltipInstance.GetComponent<RectTransform>();
            Vector2 tooltipPosition = eventData.position;
            Vector2 offset = new Vector2(-200, 0);

            tooltipInstance.transform.position = tooltipPosition + offset;
        }
    }
    private void UpdateTootipInformation()
    {
        if (tooltipInstance == null) return;
        TextMeshProUGUI tooltipText = tooltipInstance.GetComponentInChildren<TextMeshProUGUI>();
        if (tooltipText != null)
        {
            string tooltipContent = inventorySlot.item.itemName;
            if (inventorySlot.item.isStackable)
            {
                if (inventorySlot.GetStackCount()<=0)
                {
                    Destroy(tooltipInstance);
                }
                tooltipContent += $"\nCount: {inventorySlot.GetStackCount()}\n";

            }
            if (inventorySlot.item is Weapon weapon)
            {
                tooltipContent += $"\nDamage: {weapon.damage}\n";
                tooltipContent += $"\nRange: {weapon.range}\n";

                tooltipContent += $"\nElement: {weapon._weaponElement}\n";

                var bonuses = weapon.CharacteristicsBonuses();
                if (bonuses != null && bonuses.Count > 0)
                {
                    tooltipContent += "Bonuses:ò \n";
                    foreach (var bonus in bonuses)
                    {
                        tooltipContent += $"- {bonus}";
                    }
                }
            }
            else if (inventorySlot.item is Armor armor)
            {
                tooltipContent += $"\nDefense: {armor.defense}";
            }

            tooltipText.text = tooltipContent;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
        }
    }
}
