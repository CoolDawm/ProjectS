public interface ISlot
{
    Item Item { get; }
    void AddItem(Item newItem);
    void ClearSlot();
    bool CanAcceptItem(Item item);
    void SetStackCount(int count);
    int GetStackCount();
}
