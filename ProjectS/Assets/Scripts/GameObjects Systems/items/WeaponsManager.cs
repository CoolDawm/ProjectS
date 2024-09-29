using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public WeaponSlot[] weaponSlots;
    private int currentSlotIndex = 0;

    private void Start()
    {
        if (weaponSlots.Length > 0 && weaponSlots[0].GetCurrentWeapon() != null)
        {
            weaponSlots[0].AddItem(weaponSlots[0].Item); // Initialize with the first weapon
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchWeapon();
        }
    }

    private void SwitchWeapon()
    {
        if (weaponSlots[currentSlotIndex].GetCurrentWeapon() != null)
        {
            weaponSlots[currentSlotIndex].ClearInstantiatedWeapon();
        }
        // Переключаемся на следующий слот
        currentSlotIndex = (currentSlotIndex + 1) % weaponSlots.Length;

        // Включаем новое оружие в основном слоте и добавляем его характеристики
        if (weaponSlots[currentSlotIndex].GetCurrentWeapon() != null)
        {
            if (weaponSlots[currentSlotIndex].isPrimarySlot)
            {
                weaponSlots[currentSlotIndex].AddItem(weaponSlots[currentSlotIndex].GetCurrentWeapon());
            }
            else
            {
                weaponSlots[currentSlotIndex].InstantiateWeapon();
            }
        }
    }
}
