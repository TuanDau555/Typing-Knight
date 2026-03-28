
[System.Serializable]
public class ItemStack
{
    public ItemSO itemSO;
    public int quantity;

    public ItemStack(ItemSO itemSO, int quantity)
    {
        this.itemSO = itemSO;
        this.quantity = quantity;
    }
}