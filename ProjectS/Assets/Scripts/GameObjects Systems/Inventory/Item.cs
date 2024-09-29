using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public bool isStackable;
    private float expAmount=0;//expScrollOnly
    public void SetExpAmount(float amount)
    {
        expAmount = amount;
    }
    public float GetExpFromItem()
    {
        return expAmount;
    }
}
