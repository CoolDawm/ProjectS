using UnityEngine;

public class ArmorSlot : CharacterSlot
{
    public override bool CanAcceptItem(Item item)
    {
        Debug.Log("3");

        return item is Armor;
    }
}